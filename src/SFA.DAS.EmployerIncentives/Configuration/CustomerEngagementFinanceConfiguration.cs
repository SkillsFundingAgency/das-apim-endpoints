using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.Configuration
{
    public class CustomerEngagementFinanceConfiguration : ICustomerEngagementApiConfiguration
    {
        public string Url { get; set; }
        public string SubscriptionKey { get; set; }
    }
}