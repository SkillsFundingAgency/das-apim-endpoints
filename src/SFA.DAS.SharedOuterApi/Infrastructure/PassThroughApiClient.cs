using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public class PassThroughApiClient : IPassThroughApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PassThroughApiClient> _logger;

        public PassThroughApiClient(HttpClient httpClient, ILogger<PassThroughApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<InnerApiResponse> Get(Uri uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            if (queryData != null)
            {
                uri = new Uri(AddQueryString(uri.ToString(), queryData), UriKind.RelativeOrAbsolute);
            }

            var response = await _httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
            return await CreateApiResponse(response);
        }

        public Task<InnerApiResponse> Get(string uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            return Get(new Uri(uri, UriKind.RelativeOrAbsolute), queryData, cancellationToken);
        }

        public Task<InnerApiResponse> Post(string uri, CancellationToken cancellationToken = default)
        {
            return Post<object>(uri, null, cancellationToken);
        }

        public async Task<InnerApiResponse> Post<TRequest>(string uri, TRequest request, CancellationToken cancellationToken = default) where TRequest : class
        {
            var response = await _httpClient.PostAsJsonAsync(uri, request, cancellationToken).ConfigureAwait(false);
            return await CreateApiResponse(response);
        }

        public async Task<InnerApiResponse> Delete(string uri, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.DeleteAsync(uri, cancellationToken).ConfigureAwait(false);
            return await CreateApiResponse(response);
        }

        private async Task<InnerApiResponse> CreateApiResponse(HttpResponseMessage responseMessage)
        {
            return new InnerApiResponse
            {
                StatusCode = responseMessage.StatusCode,
                Json = await ReadContentAsJson(responseMessage.Content)
            };
        }

        private async Task<JsonDocument> ReadContentAsJson(HttpContent httpContent)
        {
            try
            {
                if (httpContent.Headers.ContentType.MediaType != "application/json")
                {
                    _logger.LogInformation("No json content returned");
                    return null;
                }

                await using var stream = await httpContent.ReadAsStreamAsync().ConfigureAwait(false);
                return JsonDocument.Parse(stream);
            }
            catch (Exception e)
            {
                _logger.LogError("Error reading content as json", e);
                return null;
            }
        }

        private string AddQueryString(string uri, object queryData)
        {
            var queryDataDictionary = queryData.GetType().GetProperties()
                .ToDictionary(x => x.Name, x => x.GetValue(queryData)?.ToString() ?? string.Empty);
            return QueryHelpers.AddQueryString(uri, queryDataDictionary);
        }
    }
}
