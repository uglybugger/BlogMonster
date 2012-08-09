using System.Linq;
using BlogMonster.Domain.Entities;

namespace BlogMonster.Domain.Queries
{
    public class MostRecentPostsQuery : Query<BlogPost>
    {
        private readonly int _numPosts;

        public MostRecentPostsQuery(int numPosts)
        {
            _numPosts = numPosts;
        }

        public override IQueryable<BlogPost> Filter(IQueryable<BlogPost> items)
        {
            return items
                .OrderByDescending(post => post.PostDate)
                .Take(_numPosts);
        }
    }
}