using SFA.DAS.EarlyConnect.Services.Interfaces;

namespace SFA.DAS.SharedOuterApi.Configuration
{
    public class LepsNeApiConfiguration : ILepsNeApiConfiguration
    {
        public string Url { get; set; }
        public string Identifier { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }
}
