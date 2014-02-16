using System.IO;
using System.Linq;
using System.Web.Mvc;
using BlogMonster.Web;
using ThirdDrawer.Extensions.StringExtensionMethods;

namespace MyVeryOwnBlog.Controllers
{
    public class PersonalController : Controller
    {
        public ActionResult Rss()
        {
            return new RssFeedResult(FeedSources.Personal.Feed);
        }

        public ActionResult Image(string id)
        {
            var tokens = id.Split('.');
            var mimeType = "image/{0}".FormatWith(tokens.Last()).ToLowerInvariant();

            using (var stream = FeedSources.Personal.GetStreamForImageResourceName(id))
            {
                if (stream == null) return new EmptyResult();

                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    var bytes = ms.GetBuffer().ToArray();
                    return File(bytes, mimeType, id);
                }
            }
        }

        public ActionResult Post(string id)
        {
            var syndicationItem = FeedSources.Personal.Feed.Items
                                             .Where(item => item.Id == id)
                                             .FirstOrDefault();

            return View(syndicationItem);
        }
    }
}