using System;
using SFA.DAS.Recruit.Application.Queries.GetProvider;

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

        public class ProviderTypeResponse
        {
            public short Id { get; set; }
        }

        public enum ProviderTypeIdentifier : short
        {
            MainProvider = 1,
            EmployerProvider = 2,
            SupportingProvider = 3
        }

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
