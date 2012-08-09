using System;
using System.Linq;
using BlogMonster.Domain.Entities;

namespace BlogMonster.Domain.Queries
{
    internal class GetPostBeforeQuery : QuerySingle<BlogPost>
    {
        private readonly DateTimeOffset _postDate;

        public GetPostBeforeQuery(DateTimeOffset postDate)
        {
            _postDate = postDate;
        }

        public GetPostBeforeQuery(BlogPost post)
            : this(post.PostDate)
        {
        }

        public override BlogPost Filter(IQueryable<BlogPost> items)
        {
            return items
                .OrderByDescending(item => item.PostDate)
                .Where(item => item.PostDate < _postDate)
                .FirstOrDefault();
        }
    }
}