using System;
using BlogMonster.Infrastructure;
using BlogMonster.Infrastructure.SyndicationFeedSources.Remote;

namespace BlogMonster.Configuration
{
    public class RemoteSyndicationFeedSourceBuilder
    {
        public RemoteSyndicationFeedSourceBuilder(Uri feedUri)
        {
            FeedUri = feedUri;

            CacheTimeout = TimeSpan.FromMinutes(10);
        }

        internal Uri FeedUri { get; private set; }
        internal TimeSpan CacheTimeout { get; set; }

        public RemoteSyndicationFeedSourceBuilder WithCacheTimeout(TimeSpan cacheTimeout)
        {
            CacheTimeout = cacheTimeout;
            return this;
        }

        public RemoteSyndicationFeedSource Grr()
        {
            var clock = new SystemClock();
            return new RemoteSyndicationFeedSource(clock, CacheTimeout, FeedUri);
        }
    }
}