using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Models;

namespace SFA.DAS.RoatpProviderModeration.Application.InnerApi.Responses
{
    public class GetProviderResponse
    {
        public int Ukprn { get; set; }
        public string LegalName { get; set; }
        public string MarketingInfo { get; set; }
        public ProviderType ProviderType { get; set; }
        public ProviderStatusType ProviderStatusType { get; set; }
        public DateTime? ProviderStatusUpdatedDate { get; set; }
        public bool IsProviderHasStandard { get; set; } = false;
    }
}
