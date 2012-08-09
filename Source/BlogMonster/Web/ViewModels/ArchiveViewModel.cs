using System.Collections.Generic;
using BlogMonster.Domain.Entities;

namespace BlogMonster.Web.ViewModels
{
    public class ArchiveViewModel
    {
        private readonly Dictionary<int, Dictionary<string, BlogPost[]>> _posts;

        public ArchiveViewModel(Dictionary<int, Dictionary<string, BlogPost[]>> posts)
        {
            _posts = posts;
        }

        public Dictionary<int, Dictionary<string, BlogPost[]>> Posts
        {
            get { return _posts; }
        }
    }
}