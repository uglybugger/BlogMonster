using System.Collections.Generic;
using BlogMonster.Domain.Entities;

namespace BlogMonster.Infrastructure
{
    public interface IBlogPostLoader
    {
        IEnumerable<BlogPost> LoadPosts();
    }
}