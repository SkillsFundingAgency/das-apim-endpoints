using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public abstract class GetApiClient<T> : IGetApiClient<T> where T : IApiConfiguration
    {
        protected readonly HttpClient HttpClient;
        protected readonly IWebHostEnvironment HostingEnvironment;
        protected readonly T Configuration;

        public GetApiClient(
            IHttpClientFactory httpClientFactory,
            T apiConfiguration,
            IWebHostEnvironment hostingEnvironment)
        {
            HttpClient = httpClientFactory.CreateClient();
            HostingEnvironment = hostingEnvironment;
            Configuration = apiConfiguration;
        }

        public async Task<TResponse> Get<TResponse>(IGetApiRequest request, bool ensureSuccessResponseCode = true)
        {
            await AddAuthenticationHeader();

            AddVersionHeader(request.Version);

            request.BaseUrl = Configuration.Url;
            var response = await HttpClient.GetAsync(request.GetUrl).ConfigureAwait(false);

            if (ensureSuccessResponseCode)
            {
                response.EnsureSuccessStatusCode();
            }

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TResponse>(json);
        }

        protected abstract Task AddAuthenticationHeader();

        protected abstract void AddVersionHeader(string requestVersion);
    }
}
