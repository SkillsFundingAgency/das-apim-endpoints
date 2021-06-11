using Contentful.Core.Models;

namespace SFA.DAS.Campaign.Contentful
{
    public class LandingPage : IContentType
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public SystemProperties Sys { get; set; }
        public string Slug { get; set; }
        public string HubType { get; set; }
        public string Summary { get; set; }
    }
}