using Contentful.Core.Models;
using SFA.DAS.Campaign.Interfaces;

namespace SFA.DAS.Campaign.Models
{
    public class ALandingPage : IContentType
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public SystemProperties Sys { get; set; }
        public string Slug { get; set; }
        public string HubType { get; set; }
        public string Summary { get; set; }
    }
}