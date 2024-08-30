namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetSettings
{
    public class GetSettingsResult
    {
        public int ExpiryAfterMonths { get; set; }
        public int EmployerRemovedAfterExpiryMonths { get; set; }
    }
}
