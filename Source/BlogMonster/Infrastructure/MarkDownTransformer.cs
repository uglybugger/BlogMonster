using MarkdownSharp;

namespace BlogMonster.Infrastructure
{
    public class MarkDownTransformer : IMarkDownTransformer
    {
        public string TransformToHtml(string markDown)
        {
            var transformer = CreateMarkdownTransformer();
            var result = transformer.Transform(markDown);
            return result;
        }

        private static Markdown CreateMarkdownTransformer()
        {
            var options = new MarkdownOptions
            {
                AutoHyperlink = false,
                AutoNewLines = false,
                EmptyElementSuffix = "/>",
                EncodeProblemUrlCharacters = true,
                LinkEmails = true,
                StrictBoldItalic = false,
            };
            var markdown = new Markdown(options);
            return markdown;
        }
    }
}