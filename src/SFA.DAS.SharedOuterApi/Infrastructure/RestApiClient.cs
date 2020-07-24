using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure
{
    public class RestApiClient : IRestApiClient
    {
        private readonly HttpClient _httpClient;

        public RestApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> Get<T>(Uri uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            var response = await GetResponse(uri, queryData, cancellationToken).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<T>(null, cancellationToken).ConfigureAwait(false);
        }

        public Task<T> Get<T>(string uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            return Get<T>(new Uri(uri, UriKind.RelativeOrAbsolute), queryData, cancellationToken);
        }

        public async Task<string> Get(Uri uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            var response = await GetResponse(uri, queryData, cancellationToken).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public Task<string> Get(string uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            return Get(new Uri(uri, UriKind.RelativeOrAbsolute), queryData, cancellationToken);
        }

        public async Task<HttpStatusCode> GetHttpStatusCode(string uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            var response = await GetRawResponse(new Uri(uri, UriKind.RelativeOrAbsolute), queryData, cancellationToken).ConfigureAwait(false);
            return response.StatusCode;
        }

        public Task<string> Post(string uri, CancellationToken cancellationToken = default)
        {
            return Post<object>(uri, null, cancellationToken);
        }

        public async Task<string> Post<TRequest>(string uri, TRequest request, CancellationToken cancellationToken = default) where TRequest : class
        {
            var response = await _httpClient.PostAsJsonAsync<TRequest>(new Uri(uri, UriKind.RelativeOrAbsolute), request, null, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return result;
        }

        public async Task<TResponse> Post<TRequest, TResponse>(string uri, TRequest requestData, CancellationToken cancellationToken = default) where TRequest : class where TResponse : class, new()
        {
            var response = await _httpClient.PostAsJsonAsync<TRequest>(new Uri(uri, UriKind.RelativeOrAbsolute), requestData, null, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TResponse>(null, cancellationToken).ConfigureAwait(false);

            return result;
        }

        protected virtual async Task<HttpResponseMessage> GetResponse(Uri uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            var response = await GetRawResponse(uri, queryData, cancellationToken);
            response.EnsureSuccessStatusCode();
            return response;
        }

        protected virtual async Task<HttpResponseMessage> GetRawResponse(Uri uri, object queryData = null, CancellationToken cancellationToken = default)
        {
            if (queryData != null)
            {
                uri = new Uri(AddQueryString(uri.ToString(), queryData), UriKind.RelativeOrAbsolute);
            }

            var response = await _httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
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
