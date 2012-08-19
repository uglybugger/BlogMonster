using System.Linq;
using BlogMonster.Domain.Entities;
using BlogMonster.Extensions;
using BlogMonster.Infrastructure;

namespace BlogMonster.Web.ViewModels
{
    public class BlogPostViewModel
    {
        private readonly BlogPost _post;
        private readonly BlogPost _previousPost;
        private readonly BlogPost _nextPost;
        private readonly ISiteBaseUrlProvider _siteBaseUrlProvider;

        public string Permalink
        {
            get { return "{0}/{1}/Post/{2}".FormatWith(_siteBaseUrlProvider.AbsoluteUrl, _siteBaseUrlProvider.BlogMonsterControllerRelativeUrl, _post.Permalinks.First()); }
        }

        public string DisqusIdentifier
        {
            get { return _post.Permalinks.First(); }
        }

        public string Title
        {
            get { return _post.Title; }
        }

        public string PostDate
        {
            get { return _post.PostDate.ToLocalTime().ToString("dd/MM/yyyy"); }
        }

        public string Html
        {
            get { return _post.Html; }
        }

        public string PostYear
        {
            get { return _post.PostDate.ToLocalTime().ToString("yyyy"); }
        }

        public string PostMonth
        {
            get { return _post.PostDate.ToLocalTime().ToString("MMMM"); }
        }

        public string PreviousHref
        {
            get { return _previousPost.Coalesce(p => "{0}/Post/{1}".FormatWith(_siteBaseUrlProvider.BlogMonsterControllerRelativeUrl, p.Permalinks.First()), null); }
        }

        public string PreviousTitle
        {
            get { return _previousPost.Coalesce(p => p.Title, null); }
        }

        public string NextHref
        {
            get { return _nextPost.Coalesce(p => "{0}/Post/{1}".FormatWith(_siteBaseUrlProvider.BlogMonsterControllerRelativeUrl, p.Permalinks.First()), null); }
        }

        public string NextTitle
        {
            get { return _nextPost.Coalesce(p => p.Title, null); }
        }

        public BlogPostViewModel(BlogPost post, BlogPost previousPost, BlogPost nextPost, ISiteBaseUrlProvider siteBaseUrlProvider)
        {
            _post = post;
            _previousPost = previousPost;
            _nextPost = nextPost;
            _siteBaseUrlProvider = siteBaseUrlProvider;
        }
    }
}