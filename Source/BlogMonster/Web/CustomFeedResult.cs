using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using BlogMonster.Configuration;
using BlogMonster.Domain.Entities;
using BlogMonster.Extensions;

namespace BlogMonster.Web
{
    public class CustomFeedResult : RssFeedResult
    {
        public CustomFeedResult(IEnumerable<BlogPost> items, RssFeedSettings rssFeedSettings, string siteBaseUrl)
            : base(CreateFeedFromItems(items, rssFeedSettings, siteBaseUrl))
        {
        }

        private static SyndicationFeed CreateFeedFromItems(IEnumerable<BlogPost> items, RssFeedSettings rssFeedSettings, string siteBaseUrl)
        {
            var feedItems = items
                .OrderByDescending(item => item.PostDate)
                .Select(post => new SyndicationItem(post.Title, post.Html, new Uri("{0}/blog/{1}".FormatWith(siteBaseUrl, post.Permalinks.First())))
                                    {
                                        Content = new TextSyndicationContent(post.Html, TextSyndicationContentKind.XHtml),
                                        Id = post.Permalinks.First(),
                                        PublishDate = post.PostDate,
                                        LastUpdatedTime = post.PostDate,
                                        Summary = new TextSyndicationContent(post.Html, TextSyndicationContentKind.XHtml),
                                        Title = new TextSyndicationContent(post.Title),
                                    })
                .Do(item => item.Authors.Add(rssFeedSettings.Author))
                .ToArray();

            var feed = new SyndicationFeed(feedItems)
                           {
                               Id = rssFeedSettings.FeedId,
                               Title = new TextSyndicationContent(rssFeedSettings.Title),
                               Description = new TextSyndicationContent(rssFeedSettings.Description),
                               ImageUrl = new Uri(rssFeedSettings.ImageUrl),
                               BaseUri = new Uri(siteBaseUrl),
                               Language = rssFeedSettings.Language,
                               Copyright = new TextSyndicationContent(rssFeedSettings.Copyright),
                               LastUpdatedTime = feedItems.FirstOrDefault().Coalesce(item => item.PublishDate, DateTimeOffset.MinValue),
                           };
            feed.Authors.Add(rssFeedSettings.Author);
            feed.Links.Add(new SyndicationLink(new Uri(siteBaseUrl)));

            return feed;
        }
    }
}