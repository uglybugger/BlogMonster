using System.IO;

namespace BlogMonster.Configuration
{
    public interface IEmbeddedSyndicationFeedSource : ISyndicationFeedSource
    {
        Stream GetStreamForImageResourceName(string resourceName);
    }
}