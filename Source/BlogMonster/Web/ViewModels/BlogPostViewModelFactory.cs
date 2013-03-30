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
            var disqusIdentifier = post.Permalinks.First();
            var title = post.Title;
            var permalink = "{0}{1}".FormatWith(_siteBaseUrlProvider.AbsoluteUrl, post.BuildRelativeUrl());
            var postDate = post.PostDate.ToLocalTime().ToString("dd/MM/yyyy");
            var html = post.Html;
            var postYear = post.PostDate.ToLocalTime().ToString("yyyy");
            var postMonth = post.PostDate.ToLocalTime().ToString("MMMM");
            var previousHref = previousPost.Coalesce(p => p.BuildRelativeUrl(), null);
            var previousTitle = previousPost.Coalesce(p => p.Title, null);
            var nextHref = nextPost.Coalesce(p => p.BuildRelativeUrl(), null);
            var nextTitle = nextPost.Coalesce(p => p.Title, null);

            return new BlogPostViewModel(disqusIdentifier, title, permalink, postDate, html, postYear, postMonth, previousHref, previousTitle, nextHref, nextTitle);
        }
    }
}