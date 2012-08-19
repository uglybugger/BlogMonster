using System.Linq;
using BlogMonster.Domain.Entities;

namespace BlogMonster.Domain.Queries
{
    public class GetPostByPermalinkQuery : QuerySingle<BlogPost>
    {
        private readonly string _permalink;

        public GetPostByPermalinkQuery(string permalink)
        {
            _permalink = permalink;
        }

        public override BlogPost Filter(IQueryable<BlogPost> items)
        {
            return items.FirstOrDefault(item => item.Permalinks.Any(pl => pl == _permalink));
        }
    }
}