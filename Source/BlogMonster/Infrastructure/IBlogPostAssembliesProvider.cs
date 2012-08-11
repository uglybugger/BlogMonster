using System.Collections.Generic;
using System.Reflection;

namespace BlogMonster.Infrastructure
{
    public interface IBlogPostAssembliesProvider
    {
        IEnumerable<Assembly> Assemblies { get; }
    }
}