using System;
using System.Runtime.Serialization;

namespace BlogMonster.Configuration
{
    [Serializable]
    public class BlogMonsterConfigurationException : Exception
    {
        public BlogMonsterConfigurationException()
        {
        }

        public BlogMonsterConfigurationException(string message) : base(message)
        {
        }

        public BlogMonsterConfigurationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected BlogMonsterConfigurationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}