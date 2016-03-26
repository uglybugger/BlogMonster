using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using BlogMonster.Infrastructure.SyndicationFeedSources;
using BlogMonster.Infrastructure.SyndicationFeedSources.Multiplexing;

namespace BlogMonster.Configuration
{
    public class MultiplexedBuilder
    {
        internal ISyndicationFeedSource[] SourcesToMultiplex { get; private set; }
        internal RssFeedSettings FeedSettings { get; private set; }
        internal Func<IEnumerable<SyndicationItem>, IEnumerable<SyndicationItem>> Filter { get; private set; }

        public MultiplexedBuilder(ISyndicationFeedSource[] sourcesToMultiplex)
        {
            SourcesToMultiplex = sourcesToMultiplex;
        }

        public MultiplexedBuilder WithRssSettings(RssFeedSettings feedSettings)
        {
            FeedSettings = feedSettings;
            return this;
        }

        public MultiplexedBuilder WithFilter(Func<IEnumerable<SyndicationItem>, IEnumerable<SyndicationItem>> filter)
        {
            Filter = filter;
            return this;
        }

        public ISyndicationFeedSource Grr()
        {
            return new MultiplexingFeedSource(FeedSettings, SourcesToMultiplex, Filter);
        }
    }
}