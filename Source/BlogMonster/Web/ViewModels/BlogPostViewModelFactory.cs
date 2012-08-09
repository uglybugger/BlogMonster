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
            var permalinkFor = "{0}/Home/Post/{1}".FormatWith(_siteBaseUrlProvider.BaseUrl, post.Id);
            return new BlogPostViewModel(post, previousPost, nextPost, permalinkFor);
        }
    }
}