using System.Collections.Generic;
using Contentful.Core.Models;

namespace SFA.DAS.Campaign.Contentful
{
    public class Article : IContentType
    {
        public SystemProperties Sys { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string HubType { get; set; }
        public LandingPage LandingPage { get; set; }
        public string Summary { get; set; }
        public List<ArticleSection> Sections { get; set; }
    }
}