using System.Web.Mvc;
using BlogMonster.Web;

namespace MyVeryOwnBlog.Controllers
{
    public class GitHubController: Controller
    {
        public ActionResult Rss()
        {
            return new RssFeedResult(FeedSources.GitHub.Feed);
        }
    }
}