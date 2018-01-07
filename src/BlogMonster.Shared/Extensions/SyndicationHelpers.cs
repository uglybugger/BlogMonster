using System.ServiceModel.Syndication;

namespace BlogMonster.Extensions
{
    public static class SyndicationHelpers
    {
        public static string ToHtml(this SyndicationContent content)
        {
            var textSyndicationContent = content as TextSyndicationContent;
            if (textSyndicationContent != null)
            {
                return textSyndicationContent.Text;
            }

            return content.ToString();
        }
    }
}