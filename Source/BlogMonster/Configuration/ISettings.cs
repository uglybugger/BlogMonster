using System;
using System.Collections.Generic;
using System.Reflection;
using BlogMonster.Infrastructure;

namespace BlogMonster.Configuration
{
    public interface ISettings
    {
        Assembly[] BlogPostAssemblies { get; }
        Type ControllerType { get; }
        Func<string, bool> ResourceNameFilter { get; }
        string Url { get; }
        RssFeedSettings RssFeedSettings { get; }
        IEnumerable<IBlogPostLoader> AdditionalBlogPostLoaders { get; }
    }
}