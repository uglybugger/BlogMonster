using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BlogMonster.Extensions;
using static System.String;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Embedded
{
    internal static class BlogPostExtractionExtensions
    {
        public static string ExtractId(this DateTimeOffset postDate)
        {
            var id = $"{postDate.Year:0000}.{postDate.Month:00}.{postDate.Day:00}.{postDate.Hour:00}{postDate.Minute:00}.{postDate.Offset.Hours:00}{postDate.Offset.Minutes:00}";
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

                if (!int.TryParse(slidingTokens[0], out var year)) continue;
                if (!int.TryParse(slidingTokens[1], out var month)) continue;
                if (!int.TryParse(slidingTokens[2], out var day)) continue;
                if (!int.TryParse(slidingTokens[3], out var time)) continue;
                if (!int.TryParse(slidingTokens[4], out var offset)) continue;

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
            var overrideTitleResourceName = $"{resourceBasePath}.Title.txt";
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
            var permalinkResourceName = $"{resourceBasePath}.Permalinks.txt";
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