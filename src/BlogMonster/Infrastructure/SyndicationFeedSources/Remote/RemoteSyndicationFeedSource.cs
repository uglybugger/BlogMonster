using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Xml;
using BlogMonster.Extensions;
using BlogMonster.Infrastructure.Caching;
using BlogMonster.Infrastructure.SyndicationFeedSources.Embedded;
using BlogMonster.Infrastructure.Time;

namespace BlogMonster.Infrastructure.SyndicationFeedSources.Remote
{
    public class RemoteSyndicationFeedSource : ISyndicationFeedSource
    {
        private readonly Cached<SyndicationFeed> _feed;
        private readonly Uri _feedUri;
        private readonly Func<SyndicationItem, bool> _filter;
        private readonly TimeSpan _requestTimeout;

        public RemoteSyndicationFeedSource(IClock clock, TimeSpan cacheTimeout, TimeSpan requestTimeout, Uri feedUri, Func<SyndicationItem, bool> filter)
        {
            _requestTimeout = requestTimeout;
            _feedUri = feedUri;
            _filter = filter;
            _feed = new Cached<SyndicationFeed>(cacheTimeout, clock, Fetch);
        }

        public SyndicationFeed Feed => _feed.Value;

        private SyndicationFeed Fetch()
        {
            var request = (HttpWebRequest) WebRequest.Create(_feedUri);
            request.Timeout = (int) _requestTimeout.TotalMilliseconds;
            request.UserAgent = $"BlogMonster {GetType().Assembly.GetName().Version} https://github.com/uglybugger/BlogMonster";

            HttpWebResponse response;
            string responseContent;
            try
            {
                response = (HttpWebResponse) request.GetResponse();

                using (var stream = response.GetResponseStream())
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        responseContent = streamReader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                throw new RemoteSyndicationFeedFailedException("Loading remote syndication feed timed out", ex)
                    .WithData("FeedUri", _feedUri);
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new RemoteSyndicationFeedFailedException("Loading remote syndication feed failed.")
                    .WithData("FeedUri", _feedUri)
                    .WithData("HttpStatusCode", (int) response.StatusCode)
                    .WithData("HttpStatusDescription", response.StatusDescription)
                    .WithData("ResponseContent", responseContent);
            }

            using (var stream = new StringReader(responseContent))
            {
                using (var reader = XmlReader.Create(stream))
                {
                    var feed = SyndicationFeed.Load(reader);
                    var filteredItems = feed.Items
                        .Where(item => _filter(item))
                        .ToArray();

                    var itemsField = feed.GetType().GetField("items", BindingFlags.Instance | BindingFlags.NonPublic);
                    itemsField.SetValue(feed, filteredItems);
                    return feed;
                }
            }
        }
    }
}