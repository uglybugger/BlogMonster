using System;
using System.Linq;
using BlogMonster.Domain.Entities;

namespace BlogMonster.Domain.Queries
{
    internal class GetPostAfterQuery : QuerySingle<BlogPost>
    {
        private readonly DateTimeOffset _postDate;

        public GetPostAfterQuery(BlogPost post)
            : this(post.PostDate)
        {
        }

        public GetPostAfterQuery(DateTimeOffset postDate)
        {
            _postDate = postDate;
        }

        public override BlogPost Filter(IQueryable<BlogPost> items)
        {
            return items
                .OrderBy(item => item.PostDate)
                .Where(item => item.PostDate > _postDate)
                .FirstOrDefault();
        }
    }
}