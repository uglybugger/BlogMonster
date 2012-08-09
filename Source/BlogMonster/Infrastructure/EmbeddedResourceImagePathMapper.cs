using System.Text.RegularExpressions;
using BlogMonster.Extensions;

namespace BlogMonster.Infrastructure
{
    public class EmbeddedResourceImagePathMapper : IEmbeddedResourceImagePathMapper
    {
        private static readonly Regex _pathReplacementRegex = new Regex(@"!\[(.*)]\((.*)\)", RegexOptions.Compiled);

        private readonly string _imageHandlerBaseUrl;

        public EmbeddedResourceImagePathMapper(ISiteBaseUrlProvider siteBaseUrlProvider)
        {
            _imageHandlerBaseUrl = siteBaseUrlProvider.ImageBaseUrl;
        }

        public string ReMapImagePaths(string markdown, string baseResourceName)
        {
            var result = _pathReplacementRegex.Replace(markdown, s => Evaluator(s, baseResourceName));
            return result;
        }

        private string Evaluator(Match match, string dirName)
        {
            var group2 = match.Groups[2].Value;
            if (group2.StartsWith("http://")) return match.Captures[0].Value;

            var group1 = match.Groups[1].Value;
            var replacement = "![{0}]({1}{2}.{3})".FormatWith(group1, _imageHandlerBaseUrl, dirName, group2);
            return replacement;
        }
    }
}