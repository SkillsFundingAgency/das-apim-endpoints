using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.Approvals.Api.ErrorHandling;
using SFA.DAS.Approvals.ErrorHandling;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Api.LocalDev
{
    public class LocalDevApiClient : IInternalApiClient<CommitmentsV2ApiConfiguration>
    {
        protected readonly HttpClient HttpClient;
        CommitmentsV2ApiConfiguration _configuration;
        private ILogger<LocalDevApiClient> _logger;

        public LocalDevApiClient(IHttpClientFactory factory, CommitmentsV2ApiConfiguration configuration,ILogger<LocalDevApiClient> logger)
        {
            HttpClient = factory.CreateClient();
            _configuration = configuration;
            HttpClient.BaseAddress = new Uri(_configuration.Url);
            var byteArray = Encoding.ASCII.GetBytes($"provider:password1234");
            HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
           _logger = logger;
        }

        public async Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, request.GetUrl);
            httpRequestMessage.AddVersion(request.Version);

            var response = await HttpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var errorContent = "";
            var responseBody = (TResponse)default;

            if (IsNot200RangeResponseCode(response.StatusCode))
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

        public Task Delete(IDeleteApiRequest request)
        {
            throw new NotImplementedException();
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

        public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            throw new NotImplementedException();
        }

        public Task Patch<TData>(IPatchApiRequest<TData> request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> Post<TResponse>(IPostApiRequest request)
        {
            throw new NotImplementedException();
        }

        public Task Post<TData>(IPostApiRequest<TData> request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request)
        {
            var stringContent = request.Data != null ? new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json") : null;

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, request.PostUrl);
            requestMessage.AddVersion(request.Version);
            requestMessage.Content = stringContent;

            var response = await HttpClient.SendAsync(requestMessage).ConfigureAwait(false);

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (IsNot200RangeResponseCode(response.StatusCode))
            {
                if (response.StatusCode == HttpStatusCode.BadRequest && response.GetSubStatusCode() == HttpSubStatusCode.DomainException)
                {
                    throw CreateApiModelException(response, json);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest && response.GetSubStatusCode() == HttpSubStatusCode.BulkUploadDomainException)
                {
                    throw CreateBulkUploadApiModelException(response, json);
                }
                else
                {
                    throw new RestHttpClientException(response, json);
                }
            }

            var responseBody = JsonConvert.DeserializeObject<TResponse>(json);
            var postWithResponseCode = new ApiResponse<TResponse>(responseBody, response.StatusCode, string.Empty);
            return postWithResponseCode;
        }

        private Exception CreateApiModelException(HttpResponseMessage httpResponseMessage, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.LogWarning($"{httpResponseMessage.RequestMessage.RequestUri} has returned an empty string when an array of error responses was expected.");
            }

            var errors = new DomainApimException(content);
            return errors;
        }

        private Exception CreateBulkUploadApiModelException(HttpResponseMessage httpResponseMessage, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.LogWarning($"{httpResponseMessage.RequestMessage.RequestUri} has returned an empty string when an array of error responses was expected.");
            }

            var errors = new BulkUploadApimDomainException(content);
            return errors;
        }

        public Task Put(IPutApiRequest request)
        {
            throw new NotImplementedException();
        }

        public Task Put<TData>(IPutApiRequest<TData> request)
        {
            throw new NotImplementedException();
        }
    }
}
