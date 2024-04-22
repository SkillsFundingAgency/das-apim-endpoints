using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using SFA.DAS.EarlyConnect.Services.Interfaces;
using SFA.DAS.EarlyConnect.Services.Configuration;

namespace SFA.DAS.EarlyConnect.Services.LepsApiClients
{
    public class LepsLoApiClient : ILepsLoApiClient<LepsLoApiConfiguration>
    {
        protected LepsLoApiConfiguration Configuration;
        protected readonly HttpClient HttpClient;

        public LepsLoApiClient(
            IHttpClientFactory httpClientFactory,
            LepsLoApiConfiguration apiConfiguration)
        {
            HttpClient = httpClientFactory.CreateClient();
            HttpClient.BaseAddress = new Uri(apiConfiguration.Url);
            Configuration = apiConfiguration;
        }
        public Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> Post<TResponse>(IPostApiRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
        {
            var stringContent = request.Data != null ? new StringContent(JsonSerializer.Serialize(request.Data), Encoding.UTF8, "application/json") : null;

            var queryString = $"?api-version={Configuration.apiversion}&sp={Configuration.SignedPermissions}&sv={Configuration.SignedVersion}&sig={Configuration.Signature}";

            var fullUrl = request.PostUrl + queryString;

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, fullUrl);
            requestMessage.Content = stringContent;
            requestMessage.Headers.Add("APIKey", Configuration.ApiKey);

            var response = await HttpClient.SendAsync(requestMessage).ConfigureAwait(false);

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var errorContent = "";
            var responseBody = (TResponse)default;

            if (IsNot200RangeResponseCode(response.StatusCode))
            {
                errorContent = json;
                HandleException(response, json);
            }
            else if (includeResponse)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                responseBody = JsonSerializer.Deserialize<TResponse>(json, options);
            }

            var postWithResponseCode = new ApiResponse<TResponse>(responseBody, response.StatusCode, errorContent);

            return postWithResponseCode;
        }

        public Task Post<TData>(IPostApiRequest<TData> request)
        {
            throw new NotImplementedException();
        }

        public Task Delete(IDeleteApiRequest request)
        {
            throw new NotImplementedException();
        }

        public Task Patch<TData>(IPatchApiRequest<TData> request)
        {
            throw new NotImplementedException();
        }

        public Task Put(IPutApiRequest request)
        {
            throw new NotImplementedException();
        }

        public Task Put<TData>(IPutApiRequest<TData> request)
        {
            throw new NotImplementedException();
        }
        public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
        {
            throw new NotImplementedException();
        }
        private static bool IsNot200RangeResponseCode(HttpStatusCode statusCode)
        {
            return !((int)statusCode >= 200 && (int)statusCode <= 299);
        }
        public virtual string HandleException(HttpResponseMessage response, string json)
        {
            return json;
        }
    }
}