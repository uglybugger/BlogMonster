using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using BlogMonster.Domain.Entities;
using BlogMonster.Extensions;

namespace BlogMonster.Web
{
    public class CustomFeedResult : RssFeedResult
    {
        public CustomFeedResult(IEnumerable<BlogPost> items, SyndicationPerson author)
            : base(CreateFeedFromItems(items, author))
        {
        }

        private static SyndicationFeed CreateFeedFromItems(IEnumerable<BlogPost> items, SyndicationPerson author)
        {
            var feedItems = items
                .OrderByDescending(item => item.PostDate)
                .Select(post => new SyndicationItem(post.Title, post.Html, new Uri("http://www.uglybugger.org/Home/Post/{0}".FormatWith(post.Permalinks.First())))
                                    {
                                        Content = new TextSyndicationContent(post.Html, TextSyndicationContentKind.XHtml),
                                        Id = post.Permalinks.First(),
                                        PublishDate = post.PostDate,
                                        LastUpdatedTime = post.PostDate,
                                        Summary = new TextSyndicationContent(post.Html, TextSyndicationContentKind.XHtml),
                                        Title = new TextSyndicationContent(post.Title),
                                        BaseUri = new Uri("http://www.uglybugger.org/Home/Post/{0}".FormatWith(post.Permalinks.First())),
                                    })
                .Do(item => item.Authors.Add(author))
                .ToArray();

            var feed = new SyndicationFeed(feedItems)
                           {
                               BaseUri = new Uri("http://www.uglybugger.org/Feed"),
                               Copyright = new TextSyndicationContent("Copyright (C) Andrew Harcourt. All rights reserved."),
                               Description =
                                   new TextSyndicationContent("Entertainment for those who have read the trilogy in five parts, and education for those (!) who have not..."),
                               Id = "http://www.uglybugger.org/Feed",
                               ImageUrl = new Uri("http://www.uglybugger.org/Content/Me.jpg"),
                               Language = "en-AU",
                               LastUpdatedTime = feedItems.FirstOrDefault().Coalesce(item => item.PublishDate, DateTimeOffset.MinValue),
                               Title = new TextSyndicationContent("Life, the Universe and Everything"),
                           };
            feed.Authors.Add(author);

            return feed;
        }
    }
}