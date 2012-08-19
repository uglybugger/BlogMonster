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
            return new BlogPostViewModel(post, previousPost, nextPost, _siteBaseUrlProvider);
        }
    }
}