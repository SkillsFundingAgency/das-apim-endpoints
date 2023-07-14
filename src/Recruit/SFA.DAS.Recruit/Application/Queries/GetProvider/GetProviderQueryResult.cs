using System;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Application.Queries.GetProvider
{
    public class GetProviderQueryResult
    {
        public GetProviderQueryResult()
        {
            ProviderType = new ProviderTypeResponse();
        }

        public Guid Id { get; set; }
        public long Ukprn { get; set; }
        public string LegalName { get; set; }
        public string TradingName { get; set; }
        public ProviderTypeResponse ProviderType { get; set; }

        public bool IsMainProvider => ProviderType.Id == (short)ProviderTypeIdentifier.MainProvider;
    }
}