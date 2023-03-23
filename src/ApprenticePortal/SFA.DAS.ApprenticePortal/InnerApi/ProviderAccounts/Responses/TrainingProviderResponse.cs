using System;

namespace SFA.DAS.ApprenticePortal.InnerApi.ProviderAccounts.Responses
{
    public class TrainingProviderResponse
    {
        public Guid Id { get; set; }
        public long Ukprn { get; set; }
        public string LegalName { get; set; }
        public string TradingName { get; set; }
    }
}