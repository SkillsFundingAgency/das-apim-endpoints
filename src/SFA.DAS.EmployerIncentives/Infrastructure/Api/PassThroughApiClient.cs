using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Infrastructure.Api
{
    public class PassThroughApiClient : IPassThroughApiClient
    {
        private readonly HttpClient _httpClient;

        public PassThroughApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string uri, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(new Uri(uri, UriKind.RelativeOrAbsolute), cancellationToken).ConfigureAwait(false);
            var value = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public async Task<InnerApiResponse> GetAsync(Uri uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            if (queryData != null)
            {
                uri = new Uri(AddQueryString(uri.ToString(), queryData), UriKind.RelativeOrAbsolute);
            }

            var response = await _httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
            return await CreateApiResponse(response, true);
        }

        public Task<InnerApiResponse> GetAsync(string uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            return GetAsync(new Uri(uri, UriKind.RelativeOrAbsolute), queryData, cancellationToken);
        }

        public Task<InnerApiResponse> PostAsync(string uri, bool readAsJson, CancellationToken cancellationToken = default)
        {
            return PostAsync<object>(uri, null, readAsJson, cancellationToken);
        }

        public async Task<InnerApiResponse> PostAsync<TRequest>(string uri, TRequest request, bool readAsJson, CancellationToken cancellationToken = default) where TRequest : class
        {
            var response = await _httpClient.PostAsJsonAsync(uri, request, cancellationToken).ConfigureAwait(false);
            return await CreateApiResponse(response, readAsJson);
        }

        public async Task<InnerApiResponse> DeleteAsync(string uri, bool readAsJson, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.DeleteAsync(uri, cancellationToken).ConfigureAwait(false);
            return await CreateApiResponse(response, readAsJson);
        }

        private async Task<InnerApiResponse> CreateApiResponse(HttpResponseMessage responseMessage, bool readAsJson)
        {
            return new InnerApiResponse
            {
                StatusCode = responseMessage.StatusCode,
                Json = readAsJson ? await ReadContentAsJson(responseMessage.Content) : null
            };
        }

        private async Task<JsonDocument> ReadContentAsJson(HttpContent httpContent)
        {
            try
            {
                await using var stream = await httpContent.ReadAsStreamAsync().ConfigureAwait(false);
                return JsonDocument.Parse(stream);
            }
            catch (Exception e)
            {
                throw new InvalidCastException("Error casting api response to Json", e);
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
