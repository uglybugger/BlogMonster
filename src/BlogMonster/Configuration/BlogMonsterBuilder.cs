using System;
using System.Linq;
using System.Reflection;
using BlogMonster.Infrastructure.SyndicationFeedSources;

namespace BlogMonster.Configuration
{
    public static class BlogMonsterBuilder
    {
        public static RemoteSyndicationFeedSourceBuilder FromUrl(Uri feedUri)
        {
            return new RemoteSyndicationFeedSourceBuilder(feedUri);
        }

        public static EmbeddedResourceBuilder FromEmbeddedResources(Assembly firstAssembly, params Assembly[] otherAssemblies)
        {
            var assemblies = new[] {firstAssembly}.Union(otherAssemblies).ToArray();
            return new EmbeddedResourceBuilder(assemblies);
        }

        public static MultiplexedBuilder FromOtherFeedSources(ISyndicationFeedSource feedSource, params ISyndicationFeedSource[] otherFeedSources)
        {
            var feedSources = new[] {feedSource}.Union(otherFeedSources).ToArray();
            return new MultiplexedBuilder(feedSources);
        }
    }
}