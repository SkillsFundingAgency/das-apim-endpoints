using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Interfaces;

public interface ICustomerEngagementApiConfiguration : IApiConfiguration
{
    string SubscriptionKey { get; set; }
    string CompanyName { get; set; }
    string ApiVersion { get; set; }
}