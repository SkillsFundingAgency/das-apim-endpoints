
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Models;
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Responses;

namespace SFA.DAS.RoatpProviderModeration.Application.Queries.GetProvider
{
    public class GetProviderQueryResult
    {
        public string MarketingInfo { get; set; }
        public ProviderType ProviderType { get; set; }

        public static implicit operator GetProviderQueryResult(GetProviderResponse source) =>
            new GetProviderQueryResult
            {
                MarketingInfo = source.MarketingInfo,
                ProviderType = source.ProviderType
            };
    }
}