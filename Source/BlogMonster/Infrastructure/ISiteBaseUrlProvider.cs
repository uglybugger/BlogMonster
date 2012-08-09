namespace BlogMonster.Infrastructure
{
    public interface ISiteBaseUrlProvider
    {
        string BaseUrl { get; }
        string ImageBaseUrl { get; }
    }
}