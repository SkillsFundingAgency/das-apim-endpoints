namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ICustomerEngagementApiConfiguration : IApiConfiguration
    {
        string SubscriptionKey { get; set; }
    }
}