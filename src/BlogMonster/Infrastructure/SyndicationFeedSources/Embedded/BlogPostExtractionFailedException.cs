using System;
using System.Runtime.Serialization;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Embedded
{
    [Serializable]
    public class BlogPostExtractionFailedException : Exception
    {
        public BlogPostExtractionFailedException()
        {
        }

        public BlogPostExtractionFailedException(string message) : base(message)
        {
        }

        public BlogPostExtractionFailedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected BlogPostExtractionFailedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}