using System;
using System.Threading;
using BlogMonster.Infrastructure;

namespace BlogMonster
{
    public class Cached<T> where T : class
    {
        private readonly TimeSpan _cacheTimeout;
        private readonly IClock _clock;
        private readonly Func<T> _valueFunc;
        private readonly object _mutex = new object();
        private DateTimeOffset _cachedValueGenerationTimestamp;
        private T _cachedValue;

        public Cached(TimeSpan cacheTimeout, IClock clock, Func<T> valueFunc)
        {
            _cacheTimeout = cacheTimeout;
            _clock = clock;
            _valueFunc = valueFunc;
        }

        public T Value
        {
            get
            {
                if (CachePeriodHasExpired()) _cachedValue = null;
                if (_cachedValue != null) return _cachedValue;
                lock (_mutex)
                {
                    Thread.MemoryBarrier();
                    if (_cachedValue != null) return _cachedValue;
                    _cachedValue = _valueFunc();
                    _cachedValueGenerationTimestamp = _clock.UtcNow;
                    return _cachedValue;
                }
            }
        }

        private bool CachePeriodHasExpired()
        {
            return _clock.UtcNow > (_cachedValueGenerationTimestamp + _cacheTimeout);
        }
    }
}