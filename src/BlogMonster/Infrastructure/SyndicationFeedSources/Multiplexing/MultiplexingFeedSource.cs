using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using BlogMonster.Configuration;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Multiplexing
{
    public class MultiplexingFeedSource : ISyndicationFeedSource
    {
        private readonly RssFeedSettings _feedSettings;
        private readonly ISyndicationFeedSource[] _sourcesToMultiplex;
        private readonly Func<IEnumerable<SyndicationItem>, IEnumerable<SyndicationItem>> _filter;

        internal MultiplexingFeedSource(RssFeedSettings feedSettings,
                                        ISyndicationFeedSource[] sourcesToMultiplex,
                                        Func<IEnumerable<SyndicationItem>, IEnumerable<SyndicationItem>> filter)
        {
            _feedSettings = feedSettings;
            _sourcesToMultiplex = sourcesToMultiplex;
            _filter = filter ?? (items => items);
        }

        public SyndicationFeed Feed
        {
            get
            {
                var syndicationItems = _sourcesToMultiplex
                    .SelectMany(s => s.Feed.Items);

                var filteredItems = _filter(syndicationItems)
                    .OrderByDescending(item => item.PublishDate)
                    .ToArray();

                var feed = new FeedBuilder().Build(_feedSettings, filteredItems);
                return feed;
            }
        }
    }
}