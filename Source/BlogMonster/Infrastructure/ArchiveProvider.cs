using System.Collections.Generic;
using System.Linq;
using BlogMonster.Domain.Entities;
using BlogMonster.Domain.Repositories;

namespace BlogMonster.Infrastructure
{
    public class ArchiveProvider : IArchiveProvider
    {
        private readonly Dictionary<int, Dictionary<string, BlogPost[]>> _posts;

        public ArchiveProvider(IRepository<BlogPost> repository)
        {
            _posts = repository.GetAll()
                .OrderByDescending(p => p.PostDate)
                .GroupBy(p => p.PostDate.Year)
                .ToDictionary(gy => gy.Key,
                              gy => gy.GroupBy(p => p.PostDate.ToString("MMMM"))
                                        .ToDictionary(gm => gm.Key, gm => gm.ToArray())
                );
        }

        public Dictionary<int, Dictionary<string, BlogPost[]>> Posts
        {
            get { return _posts; }
        }
    }
}