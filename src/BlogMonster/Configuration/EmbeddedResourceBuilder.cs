using System;
using System.Reflection;
using BlogMonster.Infrastructure;
using BlogMonster.Infrastructure.SyndicationFeedSources.Embedded;

namespace BlogMonster.Configuration
{
    public class EmbeddedResourceBuilder
    {
        public EmbeddedResourceBuilder(Assembly[] assemblies)
        {
            Assemblies = assemblies;
        }

        internal Assembly[] Assemblies { get; private set; }
        internal RssFeedSettings FeedSettings { get; set; }
        internal Uri BasePostUri { get; set; }
        internal Uri BaseImageUri { get; set; }
        internal Func<string, bool> BlogPostResourceNameFilter { get; set; }

        public EmbeddedResourceBuilder WithResourceNameFilter(Func<string, bool> filter)
        {
            BlogPostResourceNameFilter = filter;
            return this;
        }

        public EmbeddedResourceBuilder WithRssSettings(RssFeedSettings feedSettings)
        {
            FeedSettings = feedSettings;
            return this;
        }

        public EmbeddedResourceBuilder WithBaseUris(Uri basePostUri, Uri baseImageUri)
        {
            BasePostUri = basePostUri;
            BaseImageUri = baseImageUri;
            return this;
        }

        public IEmbeddedSyndicationFeedSource Grr()
        {
            var pathFactory = new PathFactory(BasePostUri, BaseImageUri);
            var markDownTransformer = new MarkDownTransformer();
            var imagePathMapper = new EmbeddedResourceImagePathMapper(pathFactory);
            var blogPostLoader = new EmbeddedResourceBlogPostLoader(pathFactory,
                                                                    markDownTransformer,
                                                                    Assemblies,
                                                                    FeedSettings,
                                                                    BlogPostResourceNameFilter,
                                                                    imagePathMapper);

            var feed = blogPostLoader.LoadFeed();
            var clock = new SystemClock();
            var embeddedSyndicationFeedService = new EmbeddedSyndicationFeedService(Assemblies, feed, clock);

            return embeddedSyndicationFeedService;
        }
    }
}