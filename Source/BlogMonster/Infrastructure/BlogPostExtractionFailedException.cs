using System;
using System.Runtime.Serialization;

namespace BlogMonster.Infrastructure
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