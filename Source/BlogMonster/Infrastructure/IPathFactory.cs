using System;

namespace BlogMonster.Infrastructure
{
    public interface IPathFactory
    {
        Uri GetUriForPost(string postId);
        Uri GetUriForImage(string imageResourceName);
    }
}