using System;

namespace BlogMonster.Infrastructure
{
    public interface IClock
    {
        DateTimeOffset UtcNow { get; }
    }
}