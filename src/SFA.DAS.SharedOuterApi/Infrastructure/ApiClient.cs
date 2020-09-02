using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public class ApiClient<T> : IApiClient<T> where T : IInnerApiConfiguration
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;
        private readonly T _configuration;

        public ApiClient(
            IHttpClientFactory httpClientFactory,
            T apiConfiguration,
            IWebHostEnvironment hostingEnvironment,
            IAzureClientCredentialHelper azureClientCredentialHelper)
        {
            _httpClientFactory = httpClientFactory;
            _hostingEnvironment = hostingEnvironment;
            _azureClientCredentialHelper = azureClientCredentialHelper;
            _configuration = apiConfiguration;
        }

        public async Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            var client = await GetClient(request.Version);

            request.BaseUrl = _configuration.Url;
            var response = await client.GetAsync(request.GetUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TResponse>(json);
        }

        public async Task<TResponse> Post<TResponse>(IPostApiRequest request)
        {
            var client = await GetClient(request.Version);

            request.BaseUrl = _configuration.Url;
            var stringContent = request.Data != null ? new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json") : null;

            var response = await client.PostAsync(request.PostUrl, stringContent)
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<TResponse>(json);
        }

        public async Task Delete(IDeleteApiRequest request)
        {
            var client = await GetClient(request.Version);

            request.BaseUrl = _configuration.Url;
            var response = await client.DeleteAsync(request.DeleteUrl)
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public async Task Patch<TData>(IPatchApiRequest<TData> request)
        {
            var client = await GetClient(request.Version);

            request.BaseUrl = _configuration.Url;
            var stringContent = request.Data != null ? new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json") : null;

            var response = await client.PatchAsync(request.PatchUrl, stringContent)
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public async Task Put(IPutApiRequest request)        
        {
            var client = await GetClient(request.Version);

            request.BaseUrl = _configuration.Url;
            var stringContent = request.Data != null ? new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json") : null;

            var response = await client.PutAsync(request.PutUrl, stringContent)
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public async Task Put<TData>(IPutApiRequest<TData> request)
        {
            var client = await GetClient(request.Version);

            request.BaseUrl = _configuration.Url;
            var stringContent = request.Data != null ? new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json") : null;

            var response = await client.PutAsync(request.PutUrl, stringContent)
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            var client = await GetClient(request.Version);

            request.BaseUrl = _configuration.Url;
            var response = await client.GetAsync(request.GetAllUrl).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<IEnumerable<TResponse>>(json);
        }        

        public async Task<HttpStatusCode> GetResponseCode(IGetApiRequest request, string namedClient = default)
        {
            var client = await GetClient(request.Version, namedClient);

            request.BaseUrl = _configuration.Url;
            var response = await client.GetAsync(request.GetUrl).ConfigureAwait(false);

            return response.StatusCode;
        }

        private async Task<HttpClient> GetClient(string version, string namedClient = default)
        {
            HttpClient client;
            if (string.IsNullOrEmpty(namedClient))
            {
                client =  _httpClientFactory.CreateClient();

            }
            else
            {
                client = _httpClientFactory.CreateClient(namedClient);
            }

            await AddAuthenticationHeader(client);
            AddVersionHeader(version, client);

            return client;
        }

        private async Task AddAuthenticationHeader(HttpClient client)
        {
            if (!_hostingEnvironment.IsDevelopment())
            {
                var accessToken = await _azureClientCredentialHelper.GetAccessTokenAsync(_configuration.Identifier);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        private void AddVersionHeader(string requestVersion, HttpClient client)
        {
            client.DefaultRequestHeaders.Remove("X-Version");
            client.DefaultRequestHeaders.Add("X-Version", requestVersion);
        }
    }
}
