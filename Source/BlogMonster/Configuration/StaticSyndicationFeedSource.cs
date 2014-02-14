using System;
using System.Linq;
using System.ServiceModel.Syndication;
using BlogMonster.Infrastructure;

namespace BlogMonster.Configuration
{
    public class StaticSyndicationFeedSource : ISyndicationFeedSource
    {
        private readonly SyndicationFeed _feed;
        private readonly SystemClock _clock;

        private readonly Cached<SyndicationFeed> _filteredFeed;

        public StaticSyndicationFeedSource(SyndicationFeed feed, SystemClock clock)
        {
            _feed = feed;
            _clock = clock;

            _filteredFeed = new Cached<SyndicationFeed>(TimeSpan.FromMinutes(1), _clock, () => FilterByDate(_feed));
        }

        private SyndicationFeed FilterByDate(SyndicationFeed feed)
        {
            var clone = feed.Clone(false);
            var syndicationItems = feed.Items
                                       .Where(item => item.PublishDate >= _clock.UtcNow)
                                       .ToArray();
            //clone.Items.AddRange(syndicationItems);

            return clone;
        }

        public SyndicationFeed Feed
        {
            get { return _filteredFeed.Value; }
        }
    }
}