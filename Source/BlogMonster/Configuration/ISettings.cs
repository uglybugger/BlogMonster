using System;
using System.Reflection;
using System.ServiceModel.Syndication;

namespace BlogMonster.Configuration
{
    public interface ISettings
    {
        Assembly[] BlogPostAssemblies { get; }
        Type ControllerType { get; }
        Func<string, bool> ResourceNameFilter { get; }
        SyndicationPerson Author { get; }
        string Url { get; }
    }
}