using System.Linq;
using BlogMonster.Domain.Entities;
using BlogMonster.Extensions;
using BlogMonster.Infrastructure;

namespace BlogMonster.Web.ViewModels
{
    public class BlogPostViewModelFactory
    {
        private readonly ISiteBaseUrlProvider _siteBaseUrlProvider;

        public BlogPostViewModelFactory(ISiteBaseUrlProvider siteBaseUrlProvider)
        {
            _siteBaseUrlProvider = siteBaseUrlProvider;
        }

        public BlogPostViewModel Create(BlogPost post, BlogPost previousPost, BlogPost nextPost)
        {
            string disqusIdentifier = post.Permalinks.First();
            string title = post.Title;
            string permalink = "{0}/blog/{1}".FormatWith(_siteBaseUrlProvider.AbsoluteUrl, post.Permalinks.First());
            string postDate = post.PostDate.ToLocalTime().ToString("dd/MM/yyyy");
            string html = post.Html;
            string postYear = post.PostDate.ToLocalTime().ToString("yyyy");
            string postMonth = post.PostDate.ToLocalTime().ToString("MMMM");
            string previousHref = previousPost.Coalesce(p => "/blog/{0}".FormatWith(p.Permalinks.First()), null);
            string previousTitle = previousPost.Coalesce(p => p.Title, null);
            string nextHref = nextPost.Coalesce(p => "/blog/{0}".FormatWith(p.Permalinks.First()), null);
            string nextTitle = nextPost.Coalesce(p => p.Title, null);

            return new BlogPostViewModel(disqusIdentifier, title, permalink, postDate, html, postYear, postMonth, previousHref, previousTitle, nextHref, nextTitle);
        }
    }
}