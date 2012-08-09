using System.Diagnostics;
using BlogMonster.Extensions;

namespace BlogMonster.Infrastructure
{
    public class SiteBaseUrlProvider : ISiteBaseUrlProvider
    {
        public string BaseUrl
        {
            get { return Debugger.IsAttached ? string.Empty : AbsoluteBaseUrl; }
        }

        public string ImageBaseUrl
        {
            get { return "{0}/Image/Image/".FormatWith(BaseUrl.TrimEnd('/')); }
        }

        private static string AbsoluteBaseUrl
        {
            get { return "http://www.uglybugger.org"; }
        }
    }
}