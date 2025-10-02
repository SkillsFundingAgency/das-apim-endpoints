using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using System;

namespace SFA.DAS.Approvals.Api.Models
{
    public class ProviderAccountDetailsResponse
    {
        public int Ukprn { get; set; }
        public string LegalName { get; set; }
        public string TradingName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string MarketingInfo { get; set; }
        public bool IsImported { get; set; } = false;
        public ProviderType ProviderType { get; set; }
        public ProviderStatusType ProviderStatusType { get; set; }
        public DateTime? ProviderStatusUpdatedDate { get; set; }
        public bool IsProviderHasStandard { get; set; } = false;
    }
}
