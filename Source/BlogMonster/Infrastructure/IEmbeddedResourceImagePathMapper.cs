namespace BlogMonster.Infrastructure
{
    public interface IEmbeddedResourceImagePathMapper
    {
        string ReMapImagePaths(string markdown, string baseResourceName);
    }
}