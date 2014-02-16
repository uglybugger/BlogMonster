using System.Linq;
using System.ServiceModel.Syndication;
using BlogMonster.Configuration;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Multiplexing
{
    public class MultiplexingFeedSource : ISyndicationFeedSource
    {
        private readonly RssFeedSettings _feedSettings;
        private readonly ISyndicationFeedSource[] _sourcesToMultiplex;

        internal MultiplexingFeedSource(RssFeedSettings feedSettings, ISyndicationFeedSource[] sourcesToMultiplex)
        {
            _feedSettings = feedSettings;
            _sourcesToMultiplex = sourcesToMultiplex;
        }

        public SyndicationFeed Feed
        {
            get
            {
                var syndicationItems = _sourcesToMultiplex
                    .SelectMany(s => s.Feed.Items)
                    .OrderByDescending(item => item.PublishDate)
                    .ToArray();

                var feed = new FeedBuilder().Build(_feedSettings, syndicationItems);
                return feed;
            }
        }
    }
}