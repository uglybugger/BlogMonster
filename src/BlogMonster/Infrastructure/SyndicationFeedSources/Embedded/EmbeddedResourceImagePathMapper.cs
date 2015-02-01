using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ThirdDrawer.Extensions.StringExtensionMethods;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Embedded
{
    public class EmbeddedResourceImagePathMapper : IEmbeddedResourceImagePathMapper
    {
        private readonly IPathFactory _pathFactory;
        private static readonly Regex _pathReplacementRegex = new Regex(@"!\[(.*)]\((.*)\)", RegexOptions.Compiled);

        public EmbeddedResourceImagePathMapper(IPathFactory pathFactory)
        {
            _pathFactory = pathFactory;
        }

        public string ReMapImagePaths(string markdown, string baseResourceDirectoryName, out Uri[] remappedImageUris)
        {
            var imageUris = new List<Uri>();
            var result = _pathReplacementRegex.Replace(markdown, s => Evaluator(s, baseResourceDirectoryName, imageUris));
            remappedImageUris = imageUris.ToArray();
            return result;
        }

        private string Evaluator(Match match, string baseResourceDirectoryName, List<Uri> imageUris)
        {
            var group2 = match.Groups[2].Value;
            if (group2.StartsWith("http://"))
            {
                var existingAbsoluteUri = match.Captures[0].Value;
                imageUris.Add(new Uri(existingAbsoluteUri, UriKind.RelativeOrAbsolute));
                return existingAbsoluteUri;
            }

            var imageResourceName = baseResourceDirectoryName + "." + group2;
            var uriForImage = _pathFactory.GetUriForImage(imageResourceName);
            imageUris.Add(uriForImage);

            var group1 = match.Groups[1].Value;
            var replacement = "![{0}]({1})".FormatWith(group1, uriForImage);
            return replacement;
        }
    }
}