using System.IO;

namespace BlogMonster.Infrastructure
{
    public interface IAssemblyResourceReader
    {
        Stream GetBestMatchingResourceStream(string id);
    }
}