using System;
using System.Diagnostics;

namespace BlogMonster.Infrastructure
{
    public class SystemClock : IClock
    {
        public DateTimeOffset UtcNow
        {
            get
            {
                return ShouldLieAboutDate()
                           ? DateTimeOffset.MaxValue
                           : DateTimeOffset.UtcNow;
            }
        }

        private static bool ShouldLieAboutDate()
        {
            return Debugger.IsAttached;
        }
    }
}