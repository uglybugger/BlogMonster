using System;

namespace BlogMonster.Infrastructure.Time
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }
    }
}