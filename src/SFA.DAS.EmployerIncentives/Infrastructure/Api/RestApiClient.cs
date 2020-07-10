using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Infrastructure.Api
{
    public class RestApiClient : IRestApiClient
    {
        private readonly HttpClient _httpClient;

        public RestApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(Uri uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            var response = await GetResponse(uri, queryData, cancellationToken).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<T>(null, cancellationToken).ConfigureAwait(false);
        }

        public Task<T> GetAsync<T>(string uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            return GetAsync<T>(new Uri(uri, UriKind.RelativeOrAbsolute), queryData, cancellationToken);
        }

        public async Task<string> GetAsync(Uri uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            var response = await GetResponse(uri, queryData, cancellationToken).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public Task<string> GetAsync(string uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            return GetAsync(new Uri(uri, UriKind.RelativeOrAbsolute), queryData, cancellationToken);
        }

        public Task<string> PostAsync(string uri, CancellationToken cancellationToken = default)
        {
            return PostAsync<object>(uri, null, cancellationToken);
        }

        public async Task<string> PostAsync<TRequest>(string uri, TRequest request, CancellationToken cancellationToken = default) where TRequest : class
        {
            var response = await _httpClient.PostAsJsonAsync<TRequest>(new Uri(uri, UriKind.RelativeOrAbsolute), request, null, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return result;
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest requestData, CancellationToken cancellationToken = default) where TRequest : class where TResponse : class, new()
        {
            var response = await _httpClient.PostAsJsonAsync<TRequest>(new Uri(uri, UriKind.RelativeOrAbsolute), requestData, null, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TResponse>(null, cancellationToken).ConfigureAwait(false);

            return result;
        }

        protected virtual async Task<HttpResponseMessage> GetResponse(Uri uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            if (queryData != null)
            {
                uri = new Uri(AddQueryString(uri.ToString(), queryData), UriKind.RelativeOrAbsolute);
            }

            var response = await _httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return response;
        }

        private string AddQueryString(string uri, object queryData)
        {
            var queryDataDictionary = queryData.GetType().GetProperties()
                .ToDictionary(x => x.Name, x => x.GetValue(queryData)?.ToString() ?? string.Empty);
            return QueryHelpers.AddQueryString(uri, queryDataDictionary);
        }
    }
}
