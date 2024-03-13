using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Models;
using System.Text.Json.Serialization;
using Azure;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public abstract class GetApiClient<T> : IGetApiClient<T> where T : IApiConfiguration
    {
        protected readonly HttpClient HttpClient;
        protected readonly T Configuration;

        public GetApiClient(
            IHttpClientFactory httpClientFactory,
            T apiConfiguration)
        {
            HttpClient = httpClientFactory.CreateClient();
            HttpClient.BaseAddress = new Uri(apiConfiguration.Url);
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
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, request.GetUrl);
            httpRequestMessage.AddVersion(request.Version);
            await AddAuthenticationHeader(httpRequestMessage);

            var response = await HttpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);

            return response.StatusCode;
        }

        public async Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            Uri uri;
            if (Uri.TryCreate("https://test2-refdata.apprenticeships.education.gov.uk/api/organisations/" + request.GetUrl, UriKind.Absolute, out uri))
            {
                // uri is an absolute URI
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                httpRequestMessage.AddVersion(request.Version);
                await AddAuthenticationHeader(httpRequestMessage);

                Console.WriteLine($"Sending HTTP request: {httpRequestMessage.Method} {httpRequestMessage.RequestUri}");
                if (httpRequestMessage.Headers != null)
                {
                    Console.WriteLine("RequestHEaders: ");
                    foreach (var header in httpRequestMessage.Headers)
                    {
                        Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                    }
                }

                if (httpRequestMessage.Content != null)
                {
                    Console.WriteLine($"Request Content: {await httpRequestMessage.Content.ReadAsStringAsync()}");
                }

                var response = await HttpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);

                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var errorContent = "";
                var responseBody = (TResponse)default;

                if (IsNot200RangeResponseCode(response.StatusCode))
                {
                    errorContent = json;
                }
                else if (string.IsNullOrWhiteSpace(json))
                {
                    // 204 No Content from a potential returned null
                    // Will throw if attempts to deserialise but didn't
                    // feel right making it part of the error if branch
                    // even if there is no content.
                }
                else
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    options.Converters.Add(new JsonStringEnumConverter());
                    responseBody = JsonSerializer.Deserialize<TResponse>(json, options);
                }

                var getWithResponseCode = new ApiResponse<TResponse>(responseBody, response.StatusCode, errorContent);

                return getWithResponseCode;
            }

            return null;
        }   

        private static bool IsNot200RangeResponseCode(HttpStatusCode statusCode)
        {
            return !((int)statusCode >= 200 && (int)statusCode <= 299);
        }

        protected abstract Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage);

    }
}
