using System.ServiceModel.Syndication;

namespace BlogMonster.Infrastructure.SyndicationFeedSources
{
    public interface ISyndicationFeedSource
    {
        SyndicationFeed Feed { get; }
    }
}