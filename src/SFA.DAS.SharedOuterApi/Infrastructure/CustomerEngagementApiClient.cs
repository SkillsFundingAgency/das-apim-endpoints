using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public class CustomerEngagementApiClient<T> : GetApiClient<T> where T : ICustomerEngagementApiConfiguration
    {
        public CustomerEngagementApiClient(IHttpClientFactory httpClientFactory, T apiConfiguration, IWebHostEnvironment hostingEnvironment) : base(httpClientFactory, apiConfiguration, hostingEnvironment)
        {
        }

        protected override async Task AddAuthenticationHeader()
        {
            HttpClient.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");
            HttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Configuration.SubscriptionKey);
        }

        protected override void AddVersionHeader(string requestVersion)
        {
        }
    }
}
