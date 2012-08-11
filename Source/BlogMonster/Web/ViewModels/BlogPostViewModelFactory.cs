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
            var permalink = "{0}/Home/Post/{1}".FormatWith(_siteBaseUrlProvider.BaseUrl, post.Id);
            var previousPermalink = "{0}/Home/Post/{1}".FormatWith(_siteBaseUrlProvider.BaseUrl, previousPost.Id);
            var nextPermalink = "{0}/Home/Post/{1}".FormatWith(_siteBaseUrlProvider.BaseUrl, nextPost.Id);
            return new BlogPostViewModel(post, previousPost, nextPost, permalink, previousPermalink, nextPermalink);
        }
    }
}