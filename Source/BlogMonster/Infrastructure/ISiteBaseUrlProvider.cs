namespace BlogMonster.Infrastructure
{
    public interface ISiteBaseUrlProvider
    {
        string AbsoluteUrl { get; }
        string BlogMonsterControllerRelativeUrl { get; }
        string ImageRelativeUrl { get; }
        string BlogMonsterControllerAbsoluteUrl { get; }
    }
}