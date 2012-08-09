using System;

namespace BlogMonster.Domain.Entities
{
    public class BlogPost
    {
        public string Id { get; set; }
        public DateTimeOffset PostDate { get; set; }
        public string Title { get; set; }
        public string Html { get; set; }
    }
}