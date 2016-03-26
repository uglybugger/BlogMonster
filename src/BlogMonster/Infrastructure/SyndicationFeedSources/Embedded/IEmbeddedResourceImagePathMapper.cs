using System;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Embedded
{
    public interface IEmbeddedResourceImagePathMapper
    {
        string ReMapImagePaths(string markdown, string baseResourceDirectoryName, out Uri[] remappedImageUris);
        Uri ReMapSingleImage(string imageUriOrShortResourceName, string baseResourceDirectoryName);
    }
}