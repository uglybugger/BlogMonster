using System.ServiceModel.Syndication;

namespace BlogMonster.Configuration
{
    public interface ISyndicationFeedSource
    {
        SyndicationFeed Feed { get; }
    }
}