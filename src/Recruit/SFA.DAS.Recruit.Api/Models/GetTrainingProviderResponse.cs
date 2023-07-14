using System;
using SFA.DAS.Recruit.Application.Queries.GetProvider;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Api.Models
{
    public class GetTrainingProviderResponse
    {
        public Guid Id { get; set; }
        public long Ukprn { get; set; }
        public string LegalName { get; set; }
        public string TradingName { get; set; }
        public ProviderTypeResponse ProviderType { get; set; }

        public bool IsMainProvider => ProviderType.Id == (short)ProviderTypeIdentifier.MainProvider;

        public static implicit operator GetTrainingProviderResponse(GetProviderQueryResult source)
        {
            return new GetTrainingProviderResponse
            {
                Ukprn = source.Ukprn,
                Id = source.Id,
                LegalName = source.LegalName,
                TradingName = source.TradingName,
                ProviderType = new ProviderTypeResponse
                {
                    Id = source.ProviderType.Id,
                }
            };
        }
    }
}
