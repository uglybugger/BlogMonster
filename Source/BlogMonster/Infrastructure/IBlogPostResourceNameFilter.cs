using System.Collections.Generic;

namespace BlogMonster.Infrastructure
{
    public interface IBlogPostResourceNameFilter
    {
        IEnumerable<string> Filter(IEnumerable<string> resourceNames);
    }
}