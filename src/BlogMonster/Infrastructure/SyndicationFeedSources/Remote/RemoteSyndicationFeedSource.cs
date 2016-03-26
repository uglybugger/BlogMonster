using System;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Xml;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Remote
{
    public class RemoteSyndicationFeedSource : ISyndicationFeedSource
    {
        private readonly Cached<SyndicationFeed> _feed;
        private readonly Uri _feedUri;
        private readonly Func<SyndicationItem, bool> _filter;

        public RemoteSyndicationFeedSource(IClock clock, TimeSpan cacheTimeout, Uri feedUri, Func<SyndicationItem, bool> filter)
        {
            _feedUri = feedUri;
            _filter = filter;
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
                var filteredItems = feed.Items
                                        .Where(item => _filter(item))
                                        .ToArray();

                var itemsField = feed.GetType().GetField("items", BindingFlags.Instance | BindingFlags.NonPublic);
                itemsField.SetValue(feed, filteredItems);
                return feed;
            }
        }
    }
}