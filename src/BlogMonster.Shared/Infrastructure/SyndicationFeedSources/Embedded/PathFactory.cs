using System;
using BlogMonster.Extensions;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Embedded
{
    public class PathFactory : IPathFactory
    {
        private readonly Uri _basePostUri;
        private readonly Uri _baseImageUri;

        public PathFactory(Uri basePostUri, Uri baseImageUri)
        {
            _basePostUri = basePostUri.EnsureTrailingSlash();
            _baseImageUri = baseImageUri.EnsureTrailingSlash();
        }

        public Uri GetUriForPost(string postId)
        {
            var uri = new Uri(_basePostUri, new Uri(postId, UriKind.Relative));
            return uri;
        }

        public Uri GetUriForImage(string imageResourceName)
        {
            var uri = new Uri(_baseImageUri, new Uri(imageResourceName, UriKind.Relative));

            // we put a trailing slash here because the image resource names will contain at least one . about which the
            // mvc routing engine and IIS get confused.
            var uriWithTrailingSlash = uri.EnsureTrailingSlash();

            return uriWithTrailingSlash;
        }
    }
}