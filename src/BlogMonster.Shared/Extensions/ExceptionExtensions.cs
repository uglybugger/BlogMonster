using System;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Embedded
{
    internal static class ExceptionExtensions
    {
        public static TException WithData<TException>(this TException exception, string key, object value) where TException : Exception
        {
            exception.Data[key] = value;
            return exception;
        }
    }
}