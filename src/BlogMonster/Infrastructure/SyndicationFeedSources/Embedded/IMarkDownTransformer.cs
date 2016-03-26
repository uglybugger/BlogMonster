namespace BlogMonster.Infrastructure.SyndicationFeedSources.Embedded
{
    public interface IMarkDownTransformer
    {
        string TransformToHtml(string markDown);
    }
}