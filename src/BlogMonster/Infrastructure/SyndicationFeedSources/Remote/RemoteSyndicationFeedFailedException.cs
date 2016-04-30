using System;
using System.Runtime.Serialization;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Remote
{
    [Serializable]
    public class RemoteSyndicationFeedFailedException : Exception
    {
        public RemoteSyndicationFeedFailedException()
        {
        }

        public RemoteSyndicationFeedFailedException(string message) : base(message)
        {
        }

        public RemoteSyndicationFeedFailedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected RemoteSyndicationFeedFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}