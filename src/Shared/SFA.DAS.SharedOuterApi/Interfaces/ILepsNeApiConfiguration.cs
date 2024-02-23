namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface ILepsNeApiConfiguration : IApiConfiguration
    {
        string SubscriptionKey { get; set; }
        string CompanyName { get; set; }
        string ApiVersion { get; set; }
    }
}