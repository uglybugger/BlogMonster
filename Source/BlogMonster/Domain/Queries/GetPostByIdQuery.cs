using System.Linq;
using BlogMonster.Domain.Entities;
using BlogMonster.Extensions;

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
            return items.FirstOrDefault(IsExactMatch) ?? items.FirstOrDefault(IsApproximateMatch);
        }

        private bool IsExactMatch(BlogPost item)
        {
            return item.Permalinks.Any(pl => pl == _id);
        }

        private bool IsApproximateMatch(BlogPost item)
        {
            return item.Permalinks.Any(permalink =>
                                           {
                                               var tokens = permalink.Split('.');
                                               var permalinkWithoutTimeOffset = tokens.Take(tokens.Length - 1).Join(".");
                                               return _id == permalinkWithoutTimeOffset;
                                           });
        }
    }
}