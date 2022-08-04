using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure
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
