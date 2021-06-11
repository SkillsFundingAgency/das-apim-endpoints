using Contentful.Core.Models;

namespace SFA.DAS.Campaign.Contentful
{
    public class ArticleSection : IContentType
    {
        public string Title { get; set; }
        public Document Body { get; set; }
        public SystemProperties Sys { get; set; }
        public string Slug { get; set; }
    }
}