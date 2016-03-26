using System.IO;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Embedded
{
    public interface IEmbeddedSyndicationFeedSource : ISyndicationFeedSource
    {
        Stream GetStreamForImageResourceName(string resourceName);
    }
}