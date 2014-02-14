using System.Text.RegularExpressions;
using BlogMonster.Extensions;

namespace BlogMonster.Infrastructure
{
    public class EmbeddedResourceImagePathMapper : IEmbeddedResourceImagePathMapper
    {
        private readonly IPathFactory _pathFactory;
        private static readonly Regex _pathReplacementRegex = new Regex(@"!\[(.*)]\((.*)\)", RegexOptions.Compiled);

        public EmbeddedResourceImagePathMapper(IPathFactory pathFactory)
        {
            _pathFactory = pathFactory;
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

            var imageResourceName = dirName + "." + group2;
            var uriForImage = _pathFactory.GetUriForImage(imageResourceName);

            var group1 = match.Groups[1].Value;
            var replacement = "![{0}]({1})".FormatWith(group1, uriForImage);
            return replacement;
        }
    }
}