using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BlogMonster.Domain.Entities;
using BlogMonster.Extensions;

namespace BlogMonster.Infrastructure
{
    public class BlogPostLoader
    {
        private readonly IBlogPostAssembliesProvider _blogPostAssembliesProvider;
        private readonly IBlogPostResourceNameFilter _blogPostResourceNameFilter;
        private readonly IEmbeddedResourceImagePathMapper _imagePathMapper;
        private readonly IMarkDownTransformer _markDownTransformer;

        public BlogPostLoader(IEmbeddedResourceImagePathMapper imagePathMapper,
                              IMarkDownTransformer markDownTransformer,
                              IBlogPostAssembliesProvider blogPostAssembliesProvider,
                              IBlogPostResourceNameFilter blogPostResourceNameFilter)
        {
            _imagePathMapper = imagePathMapper;
            _markDownTransformer = markDownTransformer;
            _blogPostAssembliesProvider = blogPostAssembliesProvider;
            _blogPostResourceNameFilter = blogPostResourceNameFilter;
        }

        public IEnumerable<BlogPost> LoadPosts()
        {
            var blogPosts = _blogPostAssembliesProvider.Assemblies
                .SelectMany(LoadBlogPostsFromAssembly)
                .ToArray();

            return blogPosts;
        }

        private IEnumerable<BlogPost> LoadBlogPostsFromAssembly(Assembly assembly)
        {
            var blogPosts = _blogPostResourceNameFilter.Filter(assembly.GetManifestResourceNames())
                .AsParallel()
                .Select(resourceName => TryLoadBlogPost(resourceName, assembly))
                .NotNull()
                .ToArray();

            return blogPosts;
        }

        private BlogPost TryLoadBlogPost(string resourceName, Assembly assembly)
        {
            if (!resourceName.EndsWith(".markdown")) return null;

            try
            {
                string resourceBasePath;
                DateTimeOffset postDate;
                if (!ExtractBaseResourcePathAndPostDate(resourceName, out resourceBasePath, out postDate)) return null;
                var id = ExtractId(postDate);
                var title = ExtractTitle(resourceName, resourceBasePath, assembly);
                var permalinks = ExtractPermalinks(resourceBasePath, assembly, id);
                var html = ExtractHtml(resourceName, assembly, id);

                return new BlogPost
                           {
                               Permalinks =  permalinks.ToArray(),
                               Title = title,
                               PostDate = postDate,
                               Html = html,
                           };
            }
            catch (BlogPostExtractionFailedException)
            {
                return null;
            }
        }

      
        private bool ExtractBaseResourcePathAndPostDate(string resourceName, out string resourcePath, out DateTimeOffset postDate)
        {
            var tokens = resourceName.Split('.');

            // just scan through all the tokens and see if we can pick out a set of tokens that look like a post directory
            for (var i = 0; i < tokens.Length; i++)
            {
                try
                {
                    var idTokens = tokens
                        .Skip(i)
                        .Take(5)
                        .Select(t => t.Trim('_'))
                        .ToArray();

                    var dateTokens = idTokens
                        .Select(int.Parse)
                        .ToArray();

                    var year = dateTokens[0];
                    var month = dateTokens[1];
                    var day = dateTokens[2];
                    var hour = dateTokens[3] / 100;
                    var minute = dateTokens[3] % 100;
                    var offset = new TimeSpan(dateTokens[4] / 100, dateTokens[4] % 100, 0);

                     postDate = new DateTimeOffset(year, month, day, hour, minute, 0, offset);
                    resourcePath = string.Join(".", tokens.Take(i + 5));
                    return true;
                }
                catch (Exception)
                {
                    //FIXME this is a dirty, dirty hack :(
                }
            }

            resourcePath = null;
            postDate = DateTimeOffset.MinValue;
            return false;
        }

        private string ExtractId(DateTimeOffset postDate)
        {
            var id = "{0:0000}.{1:00}.{2:00}.{3:00}{4:00}.{5:00}{6:00}".FormatWith(postDate.Year,
                                                                      postDate.Month,
                                                                      postDate.Day,
                                                                      postDate.Hour,
                                                                      postDate.Minute,
                                                                      postDate.Offset.Hours,
                                                                      postDate.Offset.Minutes);
            return id;
        }

        private string ExtractTitle(string resourceName, string resourceBasePath, Assembly assembly)
        {
            string title;
            var overrideTitleResourceName = "{0}.Title.txt".FormatWith(resourceBasePath);
            using (var stream = assembly.GetManifestResourceStream(overrideTitleResourceName))
            {
                title = stream == null
                            ? ExtractTitleFromResourceName(resourceName, resourceBasePath)
                            : new StreamReader(stream).ReadToEnd();
            }

            return title;
        }

        private static string ExtractTitleFromResourceName(string resourceName, string resourceBasePath)
        {
            var title = resourceName;
            title = title.Replace(resourceBasePath + ".", string.Empty);
            title = title.Replace(".markdown", string.Empty);
            return title;
        }

        private IEnumerable<string> ExtractPermalinks(string resourceBasePath, Assembly assembly, string id)
        {
            var permalinks = new List<string>();
            var permalinkResourceName = "{0}.Permalinks.txt".FormatWith(resourceBasePath);
            using (var stream = assembly.GetManifestResourceStream(permalinkResourceName))
            {
                if (stream != null)
                {
                    var blob = new StreamReader(stream).ReadToEnd();
                    permalinks.AddRange(blob.Split('\r', '\n'));
                }
            }
            permalinks.Add(id);

            var result = permalinks
                .Distinct()
                .NotNullOrWhitespace()
                .ToArray()
                ;
            return result;
        }


        private string ExtractHtml(string resourceName, Assembly assembly, string id)
        {
            string markdown;
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                markdown = stream == null
                               ? string.Empty
                               : new StreamReader(stream).ReadToEnd();
            }

            var markdownWithImagesRemapped = _imagePathMapper.ReMapImagePaths(markdown, id);
            var html = _markDownTransformer.TransformToHtml(markdownWithImagesRemapped);
            return html;
        }
    }
}