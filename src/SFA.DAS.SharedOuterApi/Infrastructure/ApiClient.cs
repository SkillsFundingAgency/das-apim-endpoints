using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public class ApiClient<T> : IApiClient<T> where T : IInnerApiConfiguration
    {
        private readonly HttpClient _httpClient;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;
        private readonly T _configuration;

        public ApiClient(
            IHttpClientFactory httpClientFactory,
            T apiConfiguration, 
            IHostingEnvironment hostingEnvironment,
            IAzureClientCredentialHelper azureClientCredentialHelper)
        {
            _httpClient = httpClientFactory.CreateClient();
            _hostingEnvironment = hostingEnvironment;
            _azureClientCredentialHelper = azureClientCredentialHelper;
            _configuration = apiConfiguration;
        }

        public async Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            await AddAuthenticationHeader();

            request.BaseUrl = _configuration.Url;
            var response = await _httpClient.GetAsync(request.GetUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TResponse>(json);
        }

        public async Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            await AddAuthenticationHeader();

            request.BaseUrl = _configuration.Url;
            var response = await _httpClient.GetAsync(request.GetAllUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<IEnumerable<TResponse>>(json);
        }

        private async Task AddAuthenticationHeader()
        {
            if (!_hostingEnvironment.IsDevelopment())
            {
                var accessToken = await _azureClientCredentialHelper.GetAccessTokenAsync(_configuration.Identifier);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);    
            }
        }

        public async Task<string> Ping()
        {
            var pingUrl = _configuration.Url;

            pingUrl += pingUrl.EndsWith("/") ? "ping" : "/ping";

            var response = await _httpClient.GetAsync((string) pingUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return result;
        }
    }
}
