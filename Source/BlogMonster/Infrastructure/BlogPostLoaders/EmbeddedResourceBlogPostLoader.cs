using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;
using BlogMonster.Configuration;
using BlogMonster.Extensions;

namespace BlogMonster.Infrastructure.BlogPostLoaders
{
    public class EmbeddedResourceBlogPostLoader
    {
        private readonly RssFeedSettings _feedSettings;
        private readonly Func<string, bool> _blogPostResourceNameFilter;
        private readonly IPathFactory _pathFactory;
        private readonly IMarkDownTransformer _markDownTransformer;
        private readonly Assembly[] _assemblies;
        private readonly IEmbeddedResourceImagePathMapper _imagePathMapper;

        public EmbeddedResourceBlogPostLoader(IPathFactory pathFactory,
                                              IMarkDownTransformer markDownTransformer,
                                              Assembly[] assemblies,
                                              RssFeedSettings feedSettings,
                                              Func<string, bool> blogPostResourceNameFilter, IEmbeddedResourceImagePathMapper imagePathMapper)
        {
            _pathFactory = pathFactory;
            _markDownTransformer = markDownTransformer;
            _assemblies = assemblies;

            _feedSettings = feedSettings;
            _blogPostResourceNameFilter = blogPostResourceNameFilter;
            _imagePathMapper = imagePathMapper;
        }

        public SyndicationFeed LoadFeed()
        {
            var syndicationItems = _assemblies
                .SelectMany(LoadSyndicationItemsFromAssembly)
                .OrderByDescending(p => p.PublishDate)
                .ToArray();

            var feed = new SyndicationFeed(_feedSettings.Title, _feedSettings.Description, _feedSettings.FeedHomeUri, syndicationItems)
                       {
                           Id = _feedSettings.FeedId,
                           ImageUrl = new Uri(_feedSettings.ImageUrl),
                           Language = _feedSettings.Language,
                           Copyright = new TextSyndicationContent(_feedSettings.Copyright),
                           LastUpdatedTime = syndicationItems.FirstOrDefault().Coalesce(item => item.PublishDate, DateTimeOffset.MinValue),
                       };
            feed.Authors.Add(_feedSettings.Author);
            feed.Links.Add(new SyndicationLink(_feedSettings.FeedHomeUri));

            return feed;
        }

        private IEnumerable<SyndicationItem> LoadSyndicationItemsFromAssembly(Assembly assembly)
        {
            var syndicationItems = assembly.GetManifestResourceNames()
                                           .Where(_blogPostResourceNameFilter)
                                           .Select(resourceName => TryLoadSyndicationItem(resourceName, assembly))
                                           .NotNull()
                                           .ToArray();

            return syndicationItems;
        }

        private SyndicationItem TryLoadSyndicationItem(string resourceName, Assembly assembly)
        {
            try
            {
                string resourceBasePath;
                DateTimeOffset postDate;
                if (!ExtractBaseResourcePathAndPostDate(resourceName, out resourceBasePath, out postDate)) return null;
                var resourceId = ExtractId(postDate);
                var title = ExtractTitle(resourceName, resourceBasePath, assembly);
                var internalPermalinks = ExtractInternalPermalinks(resourceBasePath, assembly, title, resourceId).ToArray();
                var id = internalPermalinks.First();
                var postUri = _pathFactory.GetUriForPost(id);
                var externalPermalinks = internalPermalinks.Select(pl => _pathFactory.GetUriForPost(pl)).ToArray();
                var html = ExtractHtml(resourceName, assembly, resourceId);

                var syndicationItem = new SyndicationItem(title, html, postUri)
                                      {
                                          Id = id,
                                          PublishDate = postDate,
                                          LastUpdatedTime = postDate,
                                          Summary = new TextSyndicationContent(html, TextSyndicationContentKind.XHtml),
                                      };

                syndicationItem.Authors.Add(_feedSettings.Author);
                syndicationItem.Links.AddRange(externalPermalinks.Select(pl => new SyndicationLink(pl)));
                return syndicationItem;
            }
            catch (BlogPostExtractionFailedException)
            {
                return null;
            }
        }

        private static bool ExtractBaseResourcePathAndPostDate(string resourceName, out string resourcePath, out DateTimeOffset postDate)
        {
            var tokens = resourceName.Split('.');

            // just scan through all the tokens and see if we can pick out a set of tokens that look like a post directory
            for (var i = 0; i < tokens.Length - 5; i++)
            {
                var slidingTokens = tokens
                    .Skip(i)
                    .Take(5)
                    .Select(t => t.Trim('_'))
                    .ToArray();

                int year;
                int month;
                int day;
                int time;
                int offset;

                if (!int.TryParse(slidingTokens[0], out year)) continue;
                if (!int.TryParse(slidingTokens[1], out month)) continue;
                if (!int.TryParse(slidingTokens[2], out day)) continue;
                if (!int.TryParse(slidingTokens[3], out time)) continue;
                if (!int.TryParse(slidingTokens[4], out offset)) continue;

                var hour = time/100;
                var minute = time%100;

                try
                {
                    var offsetTimeSpan = new TimeSpan(offset/100, offset%100, 0);
                    postDate = new DateTimeOffset(year, month, day, hour, minute, 0, offsetTimeSpan);
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

        private static string ExtractId(DateTimeOffset postDate)
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

        private static string ExtractTitle(string resourceName, string resourceBasePath, Assembly assembly)
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

        private static IEnumerable<string> ExtractInternalPermalinks(string resourceBasePath, Assembly assembly, string title, string id)
        {
            var permalinks = new List<string>();

            // add links provided by author. these take precedence
            var permalinkResourceName = "{0}.Permalinks.txt".FormatWith(resourceBasePath);
            using (var stream = assembly.GetManifestResourceStream(permalinkResourceName))
            {
                if (stream != null)
                {
                    var blob = new StreamReader(stream).ReadToEnd();
                    permalinks.AddRange(blob.Split('\r', '\n'));
                }
            }

            // see if we can figure one out
            var slug = title
                .Replace("'", string.Empty)
                .RegexReplace(@"\W", " ")
                .ToLowerInvariant()
                .Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)
                .Join("_")
                ;
            permalinks.Add(slug);

            // default to the id if nothing else
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