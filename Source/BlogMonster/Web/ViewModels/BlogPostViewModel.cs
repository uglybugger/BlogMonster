namespace BlogMonster.Web.ViewModels
{
    public class BlogPostViewModel
    {
        private readonly string _disqusIdentifier;
        private readonly string _permalink;
        private readonly string _html;
        private readonly string _nextHref;
        private readonly string _nextTitle;
        private readonly string _postDate;
        private readonly string _postMonth;
        private readonly string _postYear;
        private readonly string _previousHref;
        private readonly string _previousTitle;
        private readonly string _title;

        public BlogPostViewModel(string disqusIdentifier,
                                 string title,
                                 string permalink,
                                 string postDate,
                                 string html,
                                 string postYear,
                                 string postMonth,
                                 string previousHref,
                                 string previousTitle,
                                 string nextHref,
                                 string nextTitle)
        {
            _disqusIdentifier = disqusIdentifier;
            _title = title;
            _permalink = permalink;
            _postDate = postDate;
            _html = html;
            _postYear = postYear;
            _postMonth = postMonth;
            _previousHref = previousHref;
            _previousTitle = previousTitle;
            _nextHref = nextHref;
            _nextTitle = nextTitle;
        }

        public string DisqusIdentifier
        {
            get { return _disqusIdentifier; }
        }

        public string Title
        {
            get { return _title; }
        }

        public string Permalink
        {
            get { return _permalink; }
        }

        public string PostDate
        {
            get { return _postDate; }
        }

        public string Html
        {
            get { return _html; }
        }

        public string PostYear
        {
            get { return _postYear; }
        }

        public string PostMonth
        {
            get { return _postMonth; }
        }

        public string PreviousHref
        {
            get { return _previousHref; }
        }

        public string PreviousTitle
        {
            get { return _previousTitle; }
        }

        public string NextHref
        {
            get { return _nextHref; }
        }

        public string NextTitle
        {
            get { return _nextTitle; }
        }
    }
}