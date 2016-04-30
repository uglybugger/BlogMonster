using System;
using System.ServiceModel.Syndication;
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
            Filter = si => true;
            RequestTimeout = TimeSpan.FromMinutes(1);
        }

        internal Uri FeedUri { get; }
        internal TimeSpan CacheTimeout { get; private set; }
        internal TimeSpan RequestTimeout { get; private set; }
        internal Func<SyndicationItem, bool> Filter { get; private set; }

        public RemoteSyndicationFeedSourceBuilder WithCacheTimeout(TimeSpan cacheTimeout)
        {
            CacheTimeout = cacheTimeout;
            return this;
        }

        public RemoteSyndicationFeedSourceBuilder WithFilter(Func<SyndicationItem, bool> filter)
        {
            Filter = filter;
            return this;
        }

        public RemoteSyndicationFeedSourceBuilder WithRequestTimeout(TimeSpan requestTimeout)
        {
            RequestTimeout = requestTimeout;
            return this;
        }

        public RemoteSyndicationFeedSource Grr()
        {
            var clock = new SystemClock();
            return new RemoteSyndicationFeedSource(clock, CacheTimeout, RequestTimeout, FeedUri, Filter);
        }
    }
}