namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSettings
{
    public class GetSettingsResult
    {
        public int ExpiryAfterMonths { get; set; }
        public int RemovedAfterRequestedMonths { get; set; }
    }
}
