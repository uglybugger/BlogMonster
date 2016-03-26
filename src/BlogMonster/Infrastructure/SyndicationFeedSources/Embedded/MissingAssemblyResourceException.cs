using System;
using System.Runtime.Serialization;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Embedded
{
    [Serializable]
    public class MissingAssemblyResourceException : Exception
    {
        public MissingAssemblyResourceException()
        {
        }

        public MissingAssemblyResourceException(string message) : base(message)
        {
        }

        public MissingAssemblyResourceException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MissingAssemblyResourceException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}