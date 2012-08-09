namespace BlogMonster.Infrastructure
{
    public interface IMarkDownTransformer
    {
        string TransformToHtml(string markDown);
    }
}