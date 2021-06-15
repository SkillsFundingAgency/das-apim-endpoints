using Contentful.Core.Models;
using SFA.DAS.Campaign.Interfaces;

namespace SFA.DAS.Campaign.Models
{
    public class ArticleSection : IContentType
    {
        public string Title { get; set; }
        public Document Body { get; set; }
        public SystemProperties Sys { get; set; }
        public string Slug { get; set; }
    }
}