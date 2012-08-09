using System.Collections.Generic;
using BlogMonster.Domain.Entities;

namespace BlogMonster.Infrastructure
{
    public interface IArchiveProvider
    {
        Dictionary<int, Dictionary<string, BlogPost[]>> Posts { get; }
    }
}