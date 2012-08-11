using System;
using System.Reflection;

namespace BlogMonster.Configuration
{
    public interface ISettings
    {
        Assembly[] BlogPostAssemblies { get; }
        Type ControllerType { get; }
        Func<string, bool> ResourceNameFilter { get; }
    }
}