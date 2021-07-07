using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.Campaign.ExternalApi
{
    public class ContentfulApiClient<T> : GetApiClient<T>, IContentfulApiClient<T> where T : IContentfulApiConfiguration
    {
        public ContentfulApiClient (IHttpClientFactory httpClientFactory, T apiConfiguration, IWebHostEnvironment hostingEnvironment) : base(httpClientFactory, apiConfiguration, hostingEnvironment)
        {
        }
        
        protected override async Task AddAuthenticationHeader()
        {
            HttpClient.DefaultRequestHeaders.Remove("Authorization");
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Configuration.AccessToken}");
        }
        protected override void AddVersionHeader(string requestVersion)
        {
        }
    }
}