using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using BlogMonster.Domain.Entities;

namespace BlogMonster.Infrastructure.BlogPostLoaders
{
    public class RssBlogPostLoader : IBlogPostLoader
    {
        private readonly Uri _feedUri;

        public RssBlogPostLoader(Uri feedUri)
        {
            _feedUri = feedUri;
        }

        public IEnumerable<BlogPost> LoadPosts()
        {
            using (var reader = XmlReader.Create(_feedUri.ToString()))
            {
                var feed = SyndicationFeed.Load(reader);
                if (feed == null) return Enumerable.Empty<BlogPost>();

                var posts = feed
                    .Items
                    .Select(MapToBlogPost)
                    .ToArray();
                return posts;
            }
        }

        private static BlogPost MapToBlogPost(SyndicationItem syndicationItem)
        {
            var html = LoadContent(syndicationItem);
            var post = new BlogPost
                       {
                           Title = syndicationItem.Title.Text,
                           PostDate = syndicationItem.PublishDate,
                           Html = html,
                           Permalinks = syndicationItem.Links.Select(l => l.Uri.ToString()).ToArray(),
                       };
            return post;
        }

        private static string LoadContent(SyndicationItem syndicationItem)
        {
            var textSyndicationItem = syndicationItem.Content as TextSyndicationContent;

            if (textSyndicationItem != null) return textSyndicationItem.Text;
            if (syndicationItem.Summary != null) return syndicationItem.Summary.Text;
            return string.Empty;
        }
    }
}