using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Embedded
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
            if (Debugger.IsAttached) return feed;

            var filteredItems = feed.Items
                                    .Where(item => item.PublishDate <= _clock.UtcNow)
                                    .ToArray();
            var clone = feed.Clone(false);
            var field = clone.GetType().GetField("items", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(clone, filteredItems);

            return clone;
        }

        public SyndicationFeed Feed
        {
            get { return _filteredFeed.Value; }
        }
    }
}