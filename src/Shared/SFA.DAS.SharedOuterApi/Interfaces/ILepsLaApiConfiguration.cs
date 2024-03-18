namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ILepsLaApiConfiguration : IApiConfiguration
    {
        string SubscriptionKey { get; set; }
        string CompanyName { get; set; }
        string ApiVersion { get; set; }
    }
}