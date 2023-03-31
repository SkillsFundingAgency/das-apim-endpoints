using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService
{
    public class TrainingProviderResponse
    {
        public Guid Id { get; set; }
        public long Ukprn { get; set; }
        public string LegalName { get; set; }
        public string TradingName { get; set; }
        public ProviderTypeResponse ProviderType { get; set; }

        public class ProviderTypeResponse
        {
            public short Id { get; set; }
        }
    }
}
