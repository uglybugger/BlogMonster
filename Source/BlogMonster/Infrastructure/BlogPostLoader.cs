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
        private readonly IEmbeddedResourceImagePathMapper _imagePathMapper;
        private readonly IMarkDownTransformer _markDownTransformer;

        public BlogPostLoader(IEmbeddedResourceImagePathMapper imagePathMapper, IMarkDownTransformer markDownTransformer)
        {
            _imagePathMapper = imagePathMapper;
            _markDownTransformer = markDownTransformer;
        }

        public IEnumerable<BlogPost> LoadPosts()
        {
            var manifestResourceNames = ResourceAssembly.GetManifestResourceNames();
            var blogPosts = manifestResourceNames
                .Select(TryLoadBlogPost)
                .NotNull()
                .ToArray();

            return blogPosts;
        }

        private BlogPost TryLoadBlogPost(string resourceName)
        {
            if (!resourceName.StartsWith("Web.Posts.")) return null;
            if (!resourceName.EndsWith(".markdown")) return null;

            var tokens = resourceName.Split('.');
            if (tokens.Length < 8) return null;

            string id;
            DateTimeOffset postDate;
            try
            {
                var idTokens = tokens
                    .Skip(3)
                    .Take(4)
                    .Select(t => t.Trim('_'))
                    .ToArray();

                id = string.Join(".", idTokens);

                var dateTokens = idTokens
                    .Select(int.Parse)
                    .ToArray();

                var year = dateTokens[0];
                var month = dateTokens[1];
                var day = dateTokens[2];
                var hour = dateTokens[3] / 100;
                var minute = dateTokens[3] % 100;
                var offset = new TimeSpan(10, 0, 0);
                postDate = new DateTimeOffset(year, month, day, hour, minute, 0, offset);
            }
            catch (Exception)
            {
                return null;
            }

            string title;
            var overrideTitleResourceName =
                "{0}.Title.txt".FormatWith(string.Join(".", tokens.Skip(3).Take(4).ToArray()));
            var titleResourceName = ResourceAssembly.GetManifestResourceNames()
                .Where(rn => rn.EndsWith(overrideTitleResourceName))
                .FirstOrDefault();
            if (titleResourceName != null)
            {
                using (var stream = ResourceAssembly.GetManifestResourceStream(titleResourceName))
                {
                    // ReSharper disable AssignNullToNotNullAttribute
                    title = new StreamReader(stream).ReadToEnd();
                    // ReSharper restore AssignNullToNotNullAttribute
                }
            }
            else
            {
                title = string.Join(".", tokens.Skip(7).Take(tokens.Length - 8).ToArray());
            }

            string markdown;
            using (var stream = ResourceAssembly.GetManifestResourceStream(resourceName))
            {
                // ReSharper disable AssignNullToNotNullAttribute
                markdown = new StreamReader(stream).ReadToEnd();
                // ReSharper restore AssignNullToNotNullAttribute
            }

            var markdownWithImagesRemapped = _imagePathMapper.ReMapImagePaths(markdown,
                                                                              string.Join(".", tokens.Take(7).ToArray()));
            var html = _markDownTransformer.TransformToHtml(markdownWithImagesRemapped);

            return new BlogPost
            {
                Id = id,
                Title = title,
                PostDate = postDate,
                Html = html,
            };
        }

        private static Assembly ResourceAssembly
        {
            get { return typeof(EmbeddedResourceBlogPostRepository).Assembly; }
        }

    }
}