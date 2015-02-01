using System;
using System.ServiceModel.Syndication;

namespace BlogMonster.Configuration
{
    public class RssFeedSettings
    {
        private readonly SyndicationPerson _author;
        private readonly string _copyright;
        private readonly Uri _feedHomeUri;
        private readonly string _description;
        private readonly string _feedId;
        private readonly string _imageUrl;
        private readonly string _language;
        private readonly string _title;

        public RssFeedSettings(string feedId, string title, string description, SyndicationPerson author, string imageUrl, string language, string copyright, Uri feedHomeUri)
        {
            _author = author;
            _copyright = copyright;
            _feedHomeUri = feedHomeUri;
            _description = description;
            _feedId = feedId;
            _imageUrl = imageUrl;
            _language = language;
            _title = title;
        }

        public SyndicationPerson Author
        {
            get { return _author; }
        }

        public string Copyright
        {
            get { return _copyright; }
        }

        public string Description
        {
            get { return _description; }
        }

        public string FeedId
        {
            get { return _feedId; }
        }

        public string ImageUrl
        {
            get { return _imageUrl; }
        }

        public string Language
        {
            get { return _language; }
        }

        public string Title
        {
            get { return _title; }
        }

        public Uri FeedHomeUri
        {
            get { return _feedHomeUri; }
        }
    }
}