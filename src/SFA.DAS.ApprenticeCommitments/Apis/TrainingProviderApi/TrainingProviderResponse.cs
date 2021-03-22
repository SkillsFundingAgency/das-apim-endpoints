using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.TrainingProviderApi
{
    public class TrainingProviderResponse
    {
        public Guid Id { get; set; }
        public long Ukprn { get; set; }
        public string LegalName { get; set; }
        public string TradingName { get; set; }
    }
}
