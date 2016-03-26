using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;
using BlogMonster.Configuration;
using ThirdDrawer.Extensions.CollectionExtensionMethods;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Embedded
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
            Func<string, bool> blogPostResourceNameFilter,
            IEmbeddedResourceImagePathMapper imagePathMapper)
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

            var feed = new FeedBuilder().Build(_feedSettings, syndicationItems);

            foreach (var item in syndicationItems)
            {
                item.SourceFeed = feed;
            }

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

                if (!resourceName.ExtractBaseResourcePathAndPostDate(out resourceBasePath, out postDate)) return null;
                var resourceId = postDate.ExtractId();
                var title = assembly.ExtractTitle(resourceName, resourceBasePath);
                var internalPermalinks = assembly.ExtractInternalPermalinks(resourceBasePath, title, resourceId).ToArray();
                var id = internalPermalinks.First();
                var postUri = _pathFactory.GetUriForPost(id);
                var externalPermalinks = internalPermalinks.Select(pl => _pathFactory.GetUriForPost(pl)).ToArray();
                string summary;
                string content;
                Uri[] imageUris;
                ExtractHtml(assembly, resourceName, resourceBasePath, out summary, out content, out imageUris);

                var syndicationItem = new SyndicationItem(title, content, postUri)
                {
                    Id = id,
                    PublishDate = postDate,
                    LastUpdatedTime = postDate,
                    Summary = new TextSyndicationContent(summary, TextSyndicationContentKind.XHtml),
                };

                syndicationItem.Authors.Add(_feedSettings.Author);
                syndicationItem.Links.AddRange(externalPermalinks.Select(pl => new SyndicationLink(pl)));
                syndicationItem.Links.AddRange(imageUris.Select(uri => new SyndicationLink(uri, "image", string.Empty, string.Empty, 0)));
                return syndicationItem;
            }
            catch (BlogPostExtractionFailedException)
            {
                return null;
            }
        }

        private void ExtractHtml(Assembly assembly, string resourceName, string resourceBasePath, out string summary, out string content, out Uri[] imageUris)
        {
            string summaryMarkdown;
            string completeMarkdown;
            assembly.ExtractMarkdown(resourceName, out summaryMarkdown, out completeMarkdown);

            var explicitImageUris = assembly.ExtractSpecifiedImages(resourceBasePath)
                .Select(imageUriOrShortResourceName => _imagePathMapper.ReMapSingleImage(imageUriOrShortResourceName, resourceBasePath))
                .ToArray();

            Uri[] summaryImageUris;
            var summaryMarkdownWithImagesRemapped = _imagePathMapper.ReMapImagePaths(summaryMarkdown, resourceBasePath, out summaryImageUris);
            summary = _markDownTransformer.TransformToHtml(summaryMarkdownWithImagesRemapped);

            Uri[] contentImageUris;
            var completeMarkdownWithImagesRemapped = _imagePathMapper.ReMapImagePaths(completeMarkdown, resourceBasePath, out contentImageUris);
            content = _markDownTransformer.TransformToHtml(completeMarkdownWithImagesRemapped);

            imageUris = new Uri[0]
                .Union(explicitImageUris)
                .Union(summaryImageUris)
                .Union(contentImageUris)
                .Distinct()
                .ToArray();
        }
    }
}