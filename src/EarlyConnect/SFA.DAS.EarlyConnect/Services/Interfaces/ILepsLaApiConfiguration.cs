using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.Services.Interfaces
{
    public interface ILepsLaApiConfiguration : IApiConfiguration
    {
        string SubscriptionKey { get; set; }
        string CompanyName { get; set; }
        string ApiVersion { get; set; }
    }
}