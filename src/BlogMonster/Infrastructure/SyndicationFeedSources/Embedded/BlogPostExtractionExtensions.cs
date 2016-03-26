using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ThirdDrawer.Extensions.CollectionExtensionMethods;
using ThirdDrawer.Extensions.StringExtensionMethods;
using static System.String;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Embedded
{
    internal static class BlogPostExtractionExtensions
    {
        public static string ExtractId(this DateTimeOffset postDate)
        {
            var id = "{0:0000}.{1:00}.{2:00}.{3:00}{4:00}.{5:00}{6:00}".FormatWith(postDate.Year,
                postDate.Month,
                postDate.Day,
                postDate.Hour,
                postDate.Minute,
                postDate.Offset.Hours,
                postDate.Offset.Minutes);
            return id;
        }

        public static bool ExtractBaseResourcePathAndPostDate(this string resourceName, out string baseResourcePath, out DateTimeOffset postDate)
        {
            var tokens = resourceName.Split('.');

            // just scan through all the tokens and see if we can pick out a set of tokens that look like a post directory
            for (var i = 0; i < tokens.Length - 5; i++)
            {
                var slidingTokens = tokens
                    .Skip(i)
                    .Take(5)
                    .Select(t => t.Trim('_'))
                    .ToArray();

                int year;
                int month;
                int day;
                int time;
                int offset;

                if (!int.TryParse(slidingTokens[0], out year)) continue;
                if (!int.TryParse(slidingTokens[1], out month)) continue;
                if (!int.TryParse(slidingTokens[2], out day)) continue;
                if (!int.TryParse(slidingTokens[3], out time)) continue;
                if (!int.TryParse(slidingTokens[4], out offset)) continue;

                if (month > 12) continue;
                if (offset > 1400) continue;

                var hour = time/100;
                var minute = time%100;

                try
                {
                    var offsetTimeSpan = new TimeSpan(offset/100, offset%100, 0);
                    postDate = new DateTimeOffset(year, month, day, hour, minute, 0, offsetTimeSpan);
                    baseResourcePath = Join(".", tokens.Take(i + 5));
                    return true;
                }
                catch (Exception)
                {
                    //FIXME this is a dirty, dirty hack :(
                }
            }

            baseResourcePath = null;
            postDate = DateTimeOffset.MinValue;
            return false;
        }

        public static string ExtractTitle(this Assembly assembly, string resourceName, string resourceBasePath)
        {
            var overrideTitleResourceName = "{0}.Title.txt".FormatWith(resourceBasePath);
            var title = assembly.TryReadResource(overrideTitleResourceName) ?? ExtractTitleFromResourceName(resourceName, resourceBasePath);
            return title;
        }

        private static string ExtractTitleFromResourceName(string resourceName, string resourceBasePath)
        {
            var title = resourceName;
            title = title.Replace(resourceBasePath + ".", Empty);
            title = title.Replace(".markdown", Empty);
            return title;
        }

        public static void ExtractMarkdown(this Assembly assembly, string resourceName, out string summaryMarkdown, out string completeMarkdown)
        {
            var markdown = assembly.TryReadResource(resourceName) ?? Empty;

            var chunks = markdown.Split(new[] {"---"}, StringSplitOptions.None);
            summaryMarkdown = chunks[0];
            completeMarkdown = Join(Empty, chunks);
        }

        public static IEnumerable<string> ExtractSpecifiedImages(this Assembly assembly, string resourceBasePath)
        {
            var resourceName = $"{resourceBasePath}.Images.txt";
            var blob = assembly.TryReadResource(resourceName);
            if (string.IsNullOrWhiteSpace(blob)) return Enumerable.Empty<string>();
            var imageUris = blob.Split(Environment.NewLine.ToCharArray()).NotNullOrWhitespace().ToArray();
            return imageUris;
        }

        public static IEnumerable<string> ExtractInternalPermalinks(this Assembly assembly, string resourceBasePath, string title, string id)
        {
            var permalinks = new List<string>();

            // add links provided by author. these take precedence
            var permalinkResourceName = "{0}.Permalinks.txt".FormatWith(resourceBasePath);
            var blob = assembly.TryReadResource(permalinkResourceName);
            if (blob != null)
            {
                permalinks.AddRange(blob.Split(Environment.NewLine.ToCharArray()).NotNullOrWhitespace());
            }

            // see if we can figure one out
            var slug = title
                .Replace("'", Empty)
                .RegexReplace(@"\W", " ")
                .ToLowerInvariant()
                .Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)
                .Join("_")
                ;
            if (!IsNullOrWhiteSpace(slug)) permalinks.Add(slug);

            // default to the id if nothing else
            if (permalinks.None()) permalinks.Add(id);

            var result = permalinks
                .Distinct()
                .NotNullOrWhitespace()
                .ToArray()
                ;
            return result;
        }
    }
}