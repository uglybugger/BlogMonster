using System;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Embedded
{
    public interface IPathFactory
    {
        Uri GetUriForPost(string postId);
        Uri GetUriForImage(string imageResourceName);
    }
}