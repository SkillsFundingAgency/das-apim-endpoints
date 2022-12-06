using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Models;

namespace SFA.DAS.RoatpProviderModeration.Application.InnerApi.Responses
{
    public class GetProviderResponse
    {
        public int Ukprn { get; set; }
        public string MarketingInfo { get; set; }
        public ProviderType ProviderType { get; set; }
    }
}
