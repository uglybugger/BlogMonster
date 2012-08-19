using BlogMonster.Configuration;
using BlogMonster.Extensions;

namespace BlogMonster.Infrastructure
{
    public class SiteBaseUrlProvider : ISiteBaseUrlProvider
    {
        private readonly ISettings _settings;

        public SiteBaseUrlProvider(ISettings settings)
        {
            _settings = settings;
        }

        public string AbsoluteUrl
        {
            get { return _settings.Url; }
        }

        public string BlogMonsterControllerRelativeUrl
        {
            get { return "/{0}".FormatWith(_settings.ControllerType.Name.Replace("Controller", string.Empty)); }
        }

        public string ImageRelativeUrl
        {
            get { return "{0}/Image/".FormatWith(BlogMonsterControllerRelativeUrl.TrimEnd('/')); }
        }

        public string BlogMonsterControllerAbsoluteUrl
        {
            get { return "{0}{1}/Post".FormatWith(AbsoluteUrl, BlogMonsterControllerRelativeUrl); }
        }
    }
}