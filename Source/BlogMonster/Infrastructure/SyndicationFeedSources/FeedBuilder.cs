using System;
using System.Linq;
using System.ServiceModel.Syndication;
using BlogMonster.Configuration;
using BlogMonster.Extensions;

namespace BlogMonster.Infrastructure.SyndicationFeedSources
{
    public class FeedBuilder
    {
        public SyndicationFeed Build(RssFeedSettings settings, SyndicationItem[] syndicationItems)
        {
            var feed = new SyndicationFeed(settings.Title, settings.Description, settings.FeedHomeUri, syndicationItems)
                       {
                           Id = settings.FeedId,
                           ImageUrl = new Uri(settings.ImageUrl),
                           Language = settings.Language,
                           Copyright = new TextSyndicationContent(settings.Copyright),
                           LastUpdatedTime = syndicationItems.FirstOrDefault().Coalesce(item => item.PublishDate, DateTimeOffset.MinValue),
                       };
            feed.Authors.Add(settings.Author);
            feed.Links.Add(new SyndicationLink(settings.FeedHomeUri));

            return feed;
        }
    }
}