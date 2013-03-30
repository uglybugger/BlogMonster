using System.Linq;
using BlogMonster.Domain.Entities;

namespace BlogMonster.Extensions
{
    public static class BlogPostExtensions
    {
        public static string BuildRelativeUrl(this BlogPost post)
        {
            return "/blog/{0}/{1}/{2}/{3}".FormatWith(post.PostDate.Year, post.PostDate.Month, post.PostDate.Day, post.Permalinks.First());
        }

        public static string BuildRssId(this BlogPost post)
        {
            return "{0}.{1}.{2}.{3}".FormatWith(post.PostDate.Year, post.PostDate.Month, post.PostDate.Day, post.Permalinks.First());
        }
    }
}