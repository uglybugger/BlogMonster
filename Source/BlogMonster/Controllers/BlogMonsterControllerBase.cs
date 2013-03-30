using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using BlogMonster.Configuration;
using BlogMonster.Domain.Entities;
using BlogMonster.Domain.Queries;
using BlogMonster.Domain.Repositories;
using BlogMonster.Extensions;
using BlogMonster.Infrastructure;
using BlogMonster.Web;
using BlogMonster.Web.ViewModels;

namespace BlogMonster.Controllers
{
    public abstract class BlogMonsterControllerBase : Controller
    {
        private readonly IArchiveProvider _archiveProvider;
        private readonly IAssemblyResourceReader _assemblyResourceReader;
        private readonly BlogPostViewModelFactory _blogPostViewModelFactory;
        private readonly IRepository<BlogPost> _repository;
        private readonly ISettings _settings;
        private readonly ISiteBaseUrlProvider _siteBaseUrlProvider;

        protected BlogMonsterControllerBase()
            : this(
                ServiceLocator.BlogPostRepository,
                ServiceLocator.BlogPostViewModelFactory,
                ServiceLocator.AssemblyResourceReader,
                ServiceLocator.Settings,
                ServiceLocator.ArchiveProvider,
                ServiceLocator.SiteBaseUrlProvider)
        {
        }

        private BlogMonsterControllerBase(IRepository<BlogPost> repository,
                                          BlogPostViewModelFactory blogPostViewModelFactory,
                                          IAssemblyResourceReader assemblyResourceReader,
                                          ISettings settings,
                                          IArchiveProvider archiveProvider,
                                          ISiteBaseUrlProvider siteBaseUrlProvider)
        {
            _archiveProvider = archiveProvider;
            _siteBaseUrlProvider = siteBaseUrlProvider;
            _repository = repository;
            _blogPostViewModelFactory = blogPostViewModelFactory;
            _assemblyResourceReader = assemblyResourceReader;
            _settings = settings;
        }

        public virtual ActionResult Index()
        {
            var post = _repository.Query(new MostRecentPostsQuery(1)).First();
            return RedirectToPost(post, false);
        }

        public virtual ActionResult Index(string id)
        {
            var post = _repository.Query(new GetPostByIdQuery(id));
            return ShowPost(post);
        }

        public virtual ActionResult PostByDateAndId(int year, int month, int day, string id)
        {
            var post = _repository.Query(new GetPostByDateAndIdQuery(year, month, day, id));
            return RedirectToPost(post);
        }

        public virtual ActionResult Rss()
        {
            var items = _repository.Query(new MostRecentPostsQuery(100));
            return new CustomFeedResult(items, _settings.RssFeedSettings, _siteBaseUrlProvider.AbsoluteUrl);
        }

        public virtual ActionResult Image(string id)
        {
            // Image resource name will look something like this:
            // Posts._2009._2009._10._12._1007._1000.Screenshot1.jpg

            var tokens = id.Split('.');
            var imageName = string.Join(".", tokens.Skip(5).ToArray());
            var mimeType = "image/{0}".FormatWith(tokens.Last()).ToLowerInvariant();
            var imagePath = string.Join("._", tokens.Take(5).ToArray());
            var resourceName = "{0}.{1}".FormatWith(imagePath, imageName);
            using (var stream = _assemblyResourceReader.GetBestMatchingResourceStream(resourceName))
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

        public virtual ActionResult Archive()
        {
            var viewModel = new ArchiveViewModel(_archiveProvider.Posts);
            return PartialView("_Archive", viewModel);
        }

        protected virtual ActionResult RedirectToPost(BlogPost post, bool allowPermanentRedirect = true)
        {
            if (post == null) return Redirect("/");

            var url = post.BuildRelativeUrl();

            // we sometimes use permanent redirects so that commenting systems (e.g. disqus) will update themselves
            return allowPermanentRedirect
                       ? RedirectPermanent(url)
                       : Redirect(url);
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