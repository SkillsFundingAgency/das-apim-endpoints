using SFA.DAS.Campaign.Interfaces;

namespace SFA.DAS.Campaign.Configuration
{
    public class ContentfulApiConfiguration : IContentfulApiConfiguration
    {
        public string Url { get; set; }
        public string AccessToken { get; set; }
    }
}