using BlogMonster.Infrastructure.SyndicationFeedSources;
using BlogMonster.Infrastructure.SyndicationFeedSources.Multiplexing;

namespace BlogMonster.Configuration
{
    public class MultiplexedBuilder
    {
        public ISyndicationFeedSource[] SourcesToMultiplex { get; private set; }
        internal RssFeedSettings FeedSettings { get; set; }

        public MultiplexedBuilder(ISyndicationFeedSource[] sourcesToMultiplex)
        {
            SourcesToMultiplex = sourcesToMultiplex;
        }

        public MultiplexedBuilder WithRssSettings(RssFeedSettings feedSettings)
        {
            FeedSettings = feedSettings;
            return this;
        }

        public ISyndicationFeedSource Grr()
        {
            return new MultiplexingFeedSource(FeedSettings, SourcesToMultiplex);
        }

    }
}