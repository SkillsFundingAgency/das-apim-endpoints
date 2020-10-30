using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

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
            HttpClient.BaseAddress = new Uri(apiConfiguration.Url);
            HostingEnvironment = hostingEnvironment;
            Configuration = apiConfiguration;
        }

        public async Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            await AddAuthenticationHeader();

            AddVersionHeader(request.Version);

            var response = await HttpClient.GetAsync(request.GetUrl).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TResponse>(json);
        }

        public async Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            await AddAuthenticationHeader();

            AddVersionHeader(request.Version);

            var response = await HttpClient.GetAsync(request.GetUrl).ConfigureAwait(false);

            return response.StatusCode;
        }

        protected abstract Task AddAuthenticationHeader();

        protected abstract void AddVersionHeader(string requestVersion);
    }
}
