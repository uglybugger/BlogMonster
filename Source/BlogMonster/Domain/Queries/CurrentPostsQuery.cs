using System;
using System.Linq;
using BlogMonster.Domain.Entities;

namespace BlogMonster.Domain.Queries
{
    public class CurrentPostsQuery : Query<BlogPost>
    {
        private readonly DateTimeOffset _utcNow;

        public CurrentPostsQuery(DateTimeOffset utcNow)
        {
            _utcNow = utcNow;
        }

        public override IQueryable<BlogPost> Filter(IQueryable<BlogPost> items)
        {
            return items
                .Where(item => item.PostDate <= _utcNow)
                ;
        }
    }
}