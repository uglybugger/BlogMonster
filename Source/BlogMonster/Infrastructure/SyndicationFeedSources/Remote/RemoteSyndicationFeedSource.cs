using System;
using System.ServiceModel.Syndication;
using System.Xml;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Remote
{
    public class RemoteSyndicationFeedSource : ISyndicationFeedSource
    {
        private readonly Cached<SyndicationFeed> _feed;
        private readonly Uri _feedUri;

        public RemoteSyndicationFeedSource(IClock clock, TimeSpan cacheTimeout, Uri feedUri)
        {
            _feedUri = feedUri;
            _feed = new Cached<SyndicationFeed>(cacheTimeout, clock, Fetch);
        }

        public SyndicationFeed Feed
        {
            get { return _feed.Value; }
        }

        private SyndicationFeed Fetch()
        {
            using (var reader = XmlReader.Create(_feedUri.ToString()))
            {
                var feed = SyndicationFeed.Load(reader);
                return feed;
            }
        }
    }
}