using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Services
{
    public class CustomerEngagementApiClient<T> : GetApiClient<T>, ICustomerEngagementApiClient<T> where T : ICustomerEngagementApiConfiguration
    {
        public CustomerEngagementApiClient(IHttpClientFactory httpClientFactory, T apiConfiguration) : base(httpClientFactory, apiConfiguration)
        {
        }

        protected override async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Key", Configuration.SubscriptionKey);
        }
    }
}
