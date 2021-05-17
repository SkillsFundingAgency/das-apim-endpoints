using System;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public abstract class ApiClient<T> : GetApiClient<T>, IApiClient<T> where T : IApiConfiguration
    {
        public ApiClient(
            IHttpClientFactory httpClientFactory,
            T apiConfiguration,
            IWebHostEnvironment hostingEnvironment) : base(httpClientFactory, apiConfiguration, hostingEnvironment)
        {
        }

        [Obsolete("Use PostWithResponseCode")]
        public async Task<TResponse> Post<TResponse>(IPostApiRequest request)
        {
            var result = await PostWithResponseCode<TResponse>(request);
            
            if(IsNot200RangeResponseCode(result.StatusCode))
            {
                throw new HttpRequestContentException($"Response status code does not indicate success: {(int)result.StatusCode} ({result.StatusCode})", result.StatusCode, result.ErrorContent);
            }
            
            return result.Body;
        }

      
        public async Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request)
        {
            await AddAuthenticationHeader();

            AddVersionHeader(request.Version);

            var stringContent = request.Data != null ? new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json") : null;

            var response = await HttpClient.PostAsync(request.PostUrl, stringContent)
                .ConfigureAwait(false);

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

            var postWithResponseCode = new ApiResponse<TResponse>(responseBody, response.StatusCode, errorContent);
            
            return postWithResponseCode;
        }

        [Obsolete("Use PostWithResponseCode")]
        public async Task Post<TData>(IPostApiRequest<TData> request)
        {
            await AddAuthenticationHeader();

            AddVersionHeader(request.Version);

            var stringContent = request.Data != null ? new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json") : null;

            var response = await HttpClient.PostAsync(request.PostUrl, stringContent)
                .ConfigureAwait(false);

            await response.EnsureSuccessStatusCodeIncludeContentInException();
        }

        public async Task Delete(IDeleteApiRequest request)
        {
            await AddAuthenticationHeader();
            AddVersionHeader(request.Version);

            var response = await HttpClient.DeleteAsync(request.DeleteUrl)
                .ConfigureAwait(false);
            await response.EnsureSuccessStatusCodeIncludeContentInException();
        }

        public async Task Patch<TData>(IPatchApiRequest<TData> request)
        {
            await AddAuthenticationHeader();
            AddVersionHeader(request.Version);

            var stringContent = request.Data != null ? new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json") : null;

            var response = await HttpClient.PatchAsync(request.PatchUrl, stringContent)
                .ConfigureAwait(false);
            await response.EnsureSuccessStatusCodeIncludeContentInException();
        }

        public async Task Put(IPutApiRequest request)

        {
            await AddAuthenticationHeader();

            AddVersionHeader(request.Version);

            var stringContent = request.Data != null ? new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json") : null;

            var response = await HttpClient.PutAsync(request.PutUrl, stringContent)
                .ConfigureAwait(false);
            await response.EnsureSuccessStatusCodeIncludeContentInException();
        }

        public async Task Put<TData>(IPutApiRequest<TData> request)
        {
            await AddAuthenticationHeader();

            AddVersionHeader(request.Version);

            var stringContent = request.Data != null ? new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json") : null;

            var response = await HttpClient.PutAsync(request.PutUrl, stringContent)
                .ConfigureAwait(false);
            await response.EnsureSuccessStatusCodeIncludeContentInException();
        }

        public async Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            await AddAuthenticationHeader();
            AddVersionHeader(request.Version);
            var response = await HttpClient.GetAsync(request.GetAllUrl).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return new List<TResponse>();
            }

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<IEnumerable<TResponse>>(json);
        }

        public async Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            await AddAuthenticationHeader();
            AddVersionHeader(request.Version);
            var response = await HttpClient.GetAsync(request.GetUrl).ConfigureAwait(false);

            return response.StatusCode;
        }

        public async Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
        {
            await AddAuthenticationHeader();
            AddVersionHeader(request.Version);

            var response = await HttpClient.GetAsync(request.GetPagedUrl).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return new PagedResponse<TResponse>();
            }

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<PagedResponse<TResponse>>(json);
        }
        private static bool IsNot200RangeResponseCode(HttpStatusCode statusCode)
        {
            return !((int)statusCode >= 200 && (int)statusCode <= 299);
        }
    }
}
