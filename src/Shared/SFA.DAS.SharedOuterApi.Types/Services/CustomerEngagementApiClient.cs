using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Services;

public class CustomerEngagementApiClient<T>(IHttpClientFactory httpClientFactory, T apiConfiguration)
    : GetApiClient<T>(httpClientFactory, apiConfiguration), ICustomerEngagementApiClient<T>
    where T : ICustomerEngagementApiConfiguration
{
    protected override async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
    {
        httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Key", Configuration.SubscriptionKey);
    }
}