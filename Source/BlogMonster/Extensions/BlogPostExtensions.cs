using System.Linq;
using BlogMonster.Domain.Entities;

namespace BlogMonster.Extensions
{
    public static class BlogPostExtensions
    {
        public static string BuildRelativeUrl(this BlogPost post)
        {
            var firstPermalink = post.Permalinks.First();
            var sanitisedPermalink = firstPermalink.RemoveCharacters(":/.# \\");
            return "/blog/{0}".FormatWith(sanitisedPermalink);
        }

        public static string BuildRssId(this BlogPost post)
        {
            return "{0}".FormatWith(post.Permalinks.First());
        }
    }
}