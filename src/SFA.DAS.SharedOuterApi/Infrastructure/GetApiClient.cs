using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Models;

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
            var result = await GetWithResponseCode<TResponse>(request);
            
            if (IsNot200RangeResponseCode(result.StatusCode))
            {
                return default;
            }

            return result.Body;
        }

        public async Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            await AddAuthenticationHeader();

            AddVersionHeader(request.Version);

            var response = await HttpClient.GetAsync(request.GetUrl).ConfigureAwait(false);

            return response.StatusCode;
        }

        public async Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            await AddAuthenticationHeader();

            AddVersionHeader(request.Version);

            var response = await HttpClient.GetAsync(request.GetUrl).ConfigureAwait(false);

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            
            var errorContent = "";
            var responseBody = (TResponse)default;
            
            if(IsNot200RangeResponseCode(response.StatusCode))
            {
                errorContent = json;
            }
            else
            {
                responseBody = JsonConvert.DeserializeObject<TResponse>(json);
            }

            var getWithResponseCode = new ApiResponse<TResponse>(responseBody, response.StatusCode, errorContent);
            
            return getWithResponseCode;
        }
        
        private static bool IsNot200RangeResponseCode(HttpStatusCode statusCode)
        {
            return !((int)statusCode >= 200 && (int)statusCode <= 299);
        }

        protected abstract Task AddAuthenticationHeader();

        protected abstract void AddVersionHeader(string requestVersion);
    }
}
