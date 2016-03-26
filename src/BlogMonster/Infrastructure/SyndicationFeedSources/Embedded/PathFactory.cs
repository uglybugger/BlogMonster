using System;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Embedded
{
    public class PathFactory : IPathFactory
    {
        private readonly Uri _basePostUri;
        private readonly Uri _baseImageUri;

        public PathFactory(Uri basePostUri, Uri baseImageUri)
        {
            _basePostUri = basePostUri;
            _baseImageUri = baseImageUri;
        }

        public Uri GetUriForPost(string postId)
        {
            return new Uri(string.Format("{0}/{1}", _basePostUri, postId), UriKind.RelativeOrAbsolute);
        }

        public Uri GetUriForImage(string imageResourceName)
        {
            // we put a trailing slash here because the image resource names will contain at least one . about which the
            // mvc routing engine and IIS get confused.
            return new Uri(string.Format("{0}/{1}/", _baseImageUri, imageResourceName), UriKind.RelativeOrAbsolute);
        }
    }
}