namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ICustomerEngagementApiConfiguration : IApiConfiguration
    {
        string SubscriptionKey { get; set; }
        string CompanyName { get; set; }
        string ApiVersion { get; set; }
    }
}