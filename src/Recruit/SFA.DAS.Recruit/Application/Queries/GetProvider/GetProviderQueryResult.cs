using System;

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
    }
}