using BlogMonster.Domain.Entities;
using BlogMonster.Extensions;

namespace BlogMonster.Web.ViewModels
{
    public class BlogPostViewModel
    {
        private readonly BlogPost _post;
        private readonly BlogPost _previousPost;
        private readonly BlogPost _nextPost;
        private readonly string _permalinkFor;

        public string Permalink
        {
            get { return _permalinkFor; }
        }

        public string DisqusIdentifier
        {
            get { return _post.Id; }
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
            get { return _previousPost.Coalesce(post => _permalinkFor, null); }
        }

        public string PreviousTitle
        {
            get { return _previousPost.Coalesce(p => p.Title, null); }
        }

        public string NextHref
        {
            get { return _nextPost.Coalesce(post => _permalinkFor, null); }
        }

        public string NextTitle
        {
            get { return _nextPost.Coalesce(p => p.Title, null); }
        }


        public BlogPostViewModel(BlogPost post, BlogPost previousPost, BlogPost nextPost, string permalinkFor)
        {
            _post = post;
            _previousPost = previousPost;
            _nextPost = nextPost;
            _permalinkFor = permalinkFor;
        }
    }
}