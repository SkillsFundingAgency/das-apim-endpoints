
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Models;
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Responses;

namespace SFA.DAS.RoatpProviderModeration.Application.Queries.GetProvider
{
    public class GetProviderResult
    {
        public string MarketingInfo { get; set; }
        public ProviderType ProviderType { get; set; }

        public static implicit operator GetProviderResult(GetProviderResponse source) =>
            new GetProviderResult
            {
                MarketingInfo = source.MarketingInfo,
                ProviderType = source.ProviderType
            };
    }
}