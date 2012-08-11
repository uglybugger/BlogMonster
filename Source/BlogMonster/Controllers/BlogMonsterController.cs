using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using BlogMonster.Domain.Entities;
using BlogMonster.Domain.Queries;
using BlogMonster.Domain.Repositories;
using BlogMonster.Extensions;
using BlogMonster.Infrastructure;
using BlogMonster.Web;
using BlogMonster.Web.ViewModels;

namespace BlogMonster.Controllers
{
    public abstract class BlogMonsterController : Controller
    {
        private readonly BlogPostViewModelFactory _blogPostViewModelFactory;
        private readonly IRepository<BlogPost> _repository;
        private readonly AssemblyResourceReader _assemblyResourceReader;

        protected BlogMonsterController() : this(ServiceLocator.BlogPostRepository, ServiceLocator.BlogPostViewModelFactory, ServiceLocator.AssemblyResourceReader)
        {
        }

        private BlogMonsterController(IRepository<BlogPost> repository, BlogPostViewModelFactory blogPostViewModelFactory, AssemblyResourceReader assemblyResourceReader)
        {
            _repository = repository;
            _blogPostViewModelFactory = blogPostViewModelFactory;
            _assemblyResourceReader = assemblyResourceReader;
        }

        public virtual ActionResult Index()
        {
            var post = _repository.Query(new MostRecentPostsQuery(1)).First();
            return ShowPost(post);
        }

        public virtual ActionResult Post(string id)
        {
            var post = _repository.Query(new GetPostByIdQuery(id));
            return ShowPost(post);
        }

        public ActionResult Rss()
        {
            var items = _repository.Query(new MostRecentPostsQuery(100));
            return new CustomFeedResult(items);
        }

        public ActionResult Image(string id)
        {
            var tokens = id.Split('.');
            var imageName = string.Join(".", tokens.Skip(7).ToArray());
            var mimeType = "image/{0}".FormatWith(tokens.Last()).ToLowerInvariant();
            using (var stream = _assemblyResourceReader.GetManifestResourceStream(id))
            {
                if (stream == null) throw new InvalidOperationException();

                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    var bytes = ms.GetBuffer().ToArray();
                    return File(bytes, mimeType, imageName);
                }
            }
        }

        protected virtual ActionResult ShowPost(BlogPost post)
        {
            var previousPost = _repository.Query(new GetPostBeforeQuery(post));
            var nextPost = _repository.Query(new GetPostAfterQuery(post));

            var viewModel = _blogPostViewModelFactory.Create(post, previousPost, nextPost);
            return View("Post", viewModel);
        }
    }
}