using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.UnitTests.Infrastructure.CustomerEngagementApi
{
    public class TestCustomerEngagementApiConfiguration : ICustomerEngagementApiConfiguration
    {
        public string Url { get; set; }
        public string SubscriptionKey { get; set; }
    }
}