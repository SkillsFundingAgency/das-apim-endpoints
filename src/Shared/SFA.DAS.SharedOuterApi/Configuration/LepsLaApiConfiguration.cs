using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Configuration
{
    public class LepsLaApiConfiguration : ILepsLaExternalApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }
}
