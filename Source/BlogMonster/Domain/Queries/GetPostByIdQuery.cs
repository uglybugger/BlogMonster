using System.Linq;
using BlogMonster.Domain.Entities;

namespace BlogMonster.Domain.Queries
{
    public class GetPostByIdQuery : QuerySingle<BlogPost>
    {
        private readonly string _id;

        public GetPostByIdQuery(string id)
        {
            _id = id;
        }

        public override BlogPost Filter(IQueryable<BlogPost> items)
        {
            return items.FirstOrDefault(item => item.Id == _id);
        }
    }
}