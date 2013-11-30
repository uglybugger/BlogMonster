using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BlogMonster.Domain.Entities;
using BlogMonster.Domain.Queries;
using BlogMonster.Domain.Repositories;

namespace BlogMonster.Infrastructure
{
    public class BlogPostMemoryRepository : IRepository<BlogPost>
    {
        private readonly IClock _clock;
        private readonly List<BlogPost> _items = new List<BlogPost>();

        public BlogPostMemoryRepository(IClock clock)
        {
            _clock = clock;
        }

        public void Add(BlogPost item)
        {
            _items.Add(item);
        }

        public IEnumerable<BlogPost> GetAll()
        {
            return CurrentPosts().AsEnumerable();
        }

        public IQueryable<BlogPost> Query(Query<BlogPost> query)
        {
            return query.Filter(CurrentPosts());
        }

        public BlogPost Query(QuerySingle<BlogPost> query)
        {
            return query.Filter(CurrentPosts());
        }

        private IQueryable<BlogPost> CurrentPosts()
        {
            return ShouldLieAboutDate
                ? _items.AsQueryable()
                : new CurrentPostsQuery(_clock.UtcNow).Filter(_items.AsQueryable());
        }

        private static bool ShouldLieAboutDate
        {
            get { return Debugger.IsAttached; }
        }
    }
}