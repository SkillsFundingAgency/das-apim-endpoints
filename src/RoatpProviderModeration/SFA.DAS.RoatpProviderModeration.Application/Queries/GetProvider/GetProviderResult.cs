
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Models;
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Responses;

namespace SFA.DAS.RoatpProviderModeration.Application.Queries.GetProvider
{
    public class GetProviderResult
    {
        public int Ukprn { get; set; }
        public string LegalName { get; set; }
        public string MarketingInfo { get; set; }
        public ProviderType ProviderType { get; set; }
        public ProviderStatusType ProviderStatusType { get; set; }
        public DateTime? ProviderStatusUpdatedDate { get; set; }
        public bool IsProviderHasStandard { get; set; } = false;

        public static implicit operator GetProviderResult(GetProviderResponse source) =>
            new GetProviderResult
            {
                Ukprn = source.Ukprn,
                LegalName = source.LegalName,
                MarketingInfo = source.MarketingInfo,
                ProviderType = source.ProviderType,
                ProviderStatusType = source.ProviderStatusType,
                ProviderStatusUpdatedDate = source.ProviderStatusUpdatedDate,
                IsProviderHasStandard = source.IsProviderHasStandard 
            };
    }
}