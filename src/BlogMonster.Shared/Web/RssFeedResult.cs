
using System.ServiceModel.Syndication;
#if NETSTANDARD2_0
using Microsoft.AspNetCore.Mvc;
#else
using System.Web.Mvc;
#endif
using System.Xml;

namespace BlogMonster.Web
{
    //http://stackoverflow.com/questions/11915/rss-feeds-in-asp-net-mvc
    public class RssFeedResult : ActionResult
    {
        private readonly SyndicationFeed _feed;

        public RssFeedResult(SyndicationFeed feed)
        {
            _feed = feed;
        }


#if NETSTANDARD2_0
        public override void ExecuteResult(ActionContext context)
#else
        public override void ExecuteResult(ControllerContext context)
#endif
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";

            var rssFormatter = new Rss20FeedFormatter(_feed);

#if NETSTANDARD2_0
            using (var writer = XmlWriter.Create(context.HttpContext.Response.Body))
#else
            using (var writer = XmlWriter.Create(context.HttpContext.Response.Output))
#endif
            {
                rssFormatter.WriteTo(writer);
            }
        }
    }
}