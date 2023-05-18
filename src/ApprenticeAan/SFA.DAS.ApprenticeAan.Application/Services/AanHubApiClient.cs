using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.ApprenticeAan.Application.Services;

[ExcludeFromCodeCoverage]
public class AanHubApiClient : IAanHubApiClient<AanHubApiConfiguration>
{
    private readonly IInternalApiClient<AanHubApiConfiguration> _apiClient;

    public AanHubApiClient(IInternalApiClient<AanHubApiConfiguration> apiClient) => _apiClient = apiClient;

    public Task<TResponse> Get<TResponse>(IGetApiRequest request) => _apiClient.Get<TResponse>(request);

    public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request) => _apiClient.GetAll<TResponse>(request);

    public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request) => _apiClient.GetPaged<TResponse>(request);

    public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request) => _apiClient.GetResponseCode(request);

    public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request) => _apiClient.GetWithResponseCode<TResponse>(request);

    public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
        => _apiClient.PostWithResponseCode<TResponse>(request, includeResponse);

    public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request) => _apiClient.PatchWithResponseCode(request);

    public Task Put(IPutApiRequest request) => _apiClient.Put(request);

    public Task Put<TData>(IPutApiRequest<TData> request) => _apiClient.Put(request);

    public Task Delete(IDeleteApiRequest request) => _apiClient.Delete(request);

    public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request) => throw new NotImplementedException();

    // Methods Now Obsolete.
    public Task<TResponse> Post<TResponse>(IPostApiRequest request) => throw new NotImplementedException();

    public Task Post<TData>(IPostApiRequest<TData> request) => throw new NotImplementedException();

    public Task Patch<TData>(IPatchApiRequest<TData> request) => throw new NotImplementedException();
}

public class AanHubRestApiClient : IAanHubRestApiClient
{
    private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;
    private readonly AanHubApiConfiguration _aanHubApiConfiguration;
    private readonly HttpClient _httpClient;

    public AanHubRestApiClient(IAzureClientCredentialHelper azureClientCredentialHelper, AanHubApiConfiguration aanHubApiConfiguration, IHttpClientFactory httpClientFactory)
    {
        _azureClientCredentialHelper = azureClientCredentialHelper;
        _aanHubApiConfiguration = aanHubApiConfiguration;
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(_aanHubApiConfiguration.Url);
    }

    public async Task<RestApiResponse<TResponse>> Get<TResponse>(string url, KeyValuePair<string, string>[] headers)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        AddCustomHeaders(httpRequestMessage, headers);
        await AddAuthenticationHeader(httpRequestMessage);

        var response = await _httpClient.SendAsync(httpRequestMessage);

        var json = await response.Content.ReadAsStringAsync();

        var errorContent = string.Empty;
        var responseBody = (TResponse?)default;

        if (!response.IsSuccessStatusCode)
        {
            errorContent = json;
        }
        else
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new JsonStringEnumConverter());
            responseBody = JsonSerializer.Deserialize<TResponse>(json, options);
        }

        var getWithResponseCode = new RestApiResponse<TResponse>(responseBody, response.StatusCode, errorContent);

        return getWithResponseCode;
    }

    private async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
    {
        if (!string.IsNullOrEmpty(_aanHubApiConfiguration.Identifier))
        {
            var accessToken = await _azureClientCredentialHelper.GetAccessTokenAsync(_aanHubApiConfiguration.Identifier);
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }

    private void AddCustomHeaders(HttpRequestMessage httpRequestMessage, KeyValuePair<string, string>[] headers)
    {
        httpRequestMessage.Headers.Add("X-Version", "1.0");

        foreach (var header in headers) httpRequestMessage.Headers.Add(header.Key, header.Value);
    }
}

public record RestApiResponse<TResponse>(TResponse? Body, HttpStatusCode StatusCode, string ErrorContent);

