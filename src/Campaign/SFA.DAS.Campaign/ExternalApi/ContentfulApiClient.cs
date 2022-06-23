using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.Campaign.ExternalApi
{
    public class ContentfulApiClient<T> : GetApiClient<T>, IContentfulApiClient<T> where T : IContentfulApiConfiguration
    {
        public ContentfulApiClient (IHttpClientFactory httpClientFactory, T apiConfiguration) : base(httpClientFactory, apiConfiguration)
        {
        }
        
        protected override async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
        {
            httpRequestMessage.Headers.Add("Authorization", $"Bearer {Configuration.AccessToken}");
        }
    }
}