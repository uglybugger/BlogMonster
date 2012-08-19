using System;

namespace BlogMonster.Domain.Entities
{
    public class BlogPost
    {
        public DateTimeOffset PostDate { get; set; }
        public string[] Permalinks { get; set; }
        public string Title { get; set; }
        public string Html { get; set; }
    }
}