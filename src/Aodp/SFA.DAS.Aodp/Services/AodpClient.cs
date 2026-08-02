using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;

namespace SFA.DAS.Aodp.Services;

public interface IAodpApiClient<T> : IInternalApiClient<T>
{
    Task<ApiResponse<TResponse>> PostWithResponseCodeAsMultipart<TResponse>(
        IPostMultipartFormDataApiRequest request,
        CancellationToken cancellationToken = default);
    Task<ApiResponse<TResponse>> PostWithResponseCodeAsJsonFile<TResponse>(
        IPostMultipartJsonFileApiRequest request,
        CancellationToken cancellationToken = default);
}

public class AodpApiClient : IAodpApiClient<AodpApiConfiguration>
{
    private readonly IInternalApiClient<AodpApiConfiguration> _apiClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AodpApiConfiguration _configuration;
    private readonly IAzureClientCredentialHelper _azureClientCredentialHelper;

    public AodpApiClient(
        IInternalApiClient<AodpApiConfiguration> apiClient,
        IHttpClientFactory httpClientFactory,
        AodpApiConfiguration configuration,
        IAzureClientCredentialHelper azureClientCredentialHelper)
    {
        _apiClient = apiClient;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _azureClientCredentialHelper = azureClientCredentialHelper;
    }

    public Task<TResponse> Get<TResponse>(IGetApiRequest request)
    {
        return _apiClient.Get<TResponse>(request);
    }

    public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
    {
        return _apiClient.GetResponseCode(request);
    }

    public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
    {
        return _apiClient.GetWithResponseCode<TResponse>(request);
    }

    public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
    {
        throw new System.NotImplementedException();
    }

    public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
    {
        throw new System.NotImplementedException();
    }

    public Task<TResponse> Post<TResponse>(IPostApiRequest request)
    {
        throw new System.NotImplementedException();
    }

    public Task Post<TData>(IPostApiRequest<TData> request)
    {
        throw new System.NotImplementedException();
    }

    public Task Delete(IDeleteApiRequest request)
    {
        return _apiClient.Delete(request);
    }

    public Task<ApiResponse<TResponse>> DeleteWithResponseCode<TResponse>(IDeleteApiRequest request,
        bool includeResponse = false)
    {
        return _apiClient.DeleteWithResponseCode<TResponse>(request, includeResponse);
    }

    public Task Patch<TData>(IPatchApiRequest<TData> request)
    {
        throw new System.NotImplementedException();
    }

    public async Task Put(IPutApiRequest request)
    {
        await _apiClient.Put(request);
    }

    public Task Put<TData>(IPutApiRequest<TData> request)
    {
        throw new System.NotImplementedException();
    }

    public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request,
        bool includeResponse = true)
    {
        return _apiClient.PostWithResponseCode<TResponse>(request);
    }

    public async Task<ApiResponse<TResponse>> PostWithResponseCodeAsMultipart<TResponse>(
        IPostMultipartFormDataApiRequest request,
        CancellationToken cancellationToken = default)
    {
        using var multipartContent = new MultipartFormDataContent();
        foreach (var field in request.FormData)
        {
            multipartContent.Add(new StringContent(field.Value, Encoding.UTF8), field.Key);
        }

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, request.PostUrl)
        {
            Content = multipartContent
        };
        requestMessage.Headers.Add("X-Version", "1.0");

        if (!string.IsNullOrWhiteSpace(_configuration.Identifier))
        {
            var accessToken = await _azureClientCredentialHelper
                .GetAccessTokenAsync(_configuration.Identifier);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(_configuration.Url);

        using var response = await client.SendAsync(requestMessage, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var body = string.IsNullOrWhiteSpace(responseContent)
            ? default
            : JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return new ApiResponse<TResponse>(body!, response.StatusCode, responseContent);
    }

    public async Task<ApiResponse<TResponse>> PostWithResponseCodeAsJsonFile<TResponse>(
        IPostMultipartJsonFileApiRequest request,
        CancellationToken cancellationToken = default)
    {
        using var multipartContent = CreateWafCompatibleMultipartContent();
        var jsonContent = new StringContent(
            JsonSerializer.Serialize(request.Data),
            Encoding.UTF8,
            "application/json");
        multipartContent.Add(jsonContent, "payload", "payload.json");

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, request.PostUrl)
        {
            Content = multipartContent
        };
        requestMessage.Headers.Add("X-Version", "1.0");

        if (!string.IsNullOrWhiteSpace(_configuration.Identifier))
        {
            var accessToken = await _azureClientCredentialHelper
                .GetAccessTokenAsync(_configuration.Identifier);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(_configuration.Url);

        using var response = await client.SendAsync(requestMessage, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var body = string.IsNullOrWhiteSpace(responseContent)
            ? default
            : JsonSerializer.Deserialize<TResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return new ApiResponse<TResponse>(body!, response.StatusCode, responseContent);
    }

    private static MultipartFormDataContent CreateWafCompatibleMultipartContent()
    {
        var boundary = $"---------------------------{Guid.NewGuid():N}";
        var content = new MultipartFormDataContent(boundary);
        var boundaryParameter = content.Headers.ContentType?.Parameters
            .Single(parameter => string.Equals(parameter.Name, "boundary", StringComparison.OrdinalIgnoreCase));

        if (boundaryParameter is not null)
        {
            boundaryParameter.Value = boundary;
        }

        return content;
    }

    public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
    {
        return _apiClient.PatchWithResponseCode<TData>(request);
    }

    public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request) where TResponse : class
    {
        return _apiClient.PutWithResponseCode<TResponse>(request);
    }

    public Task<ApiResponse<TResponse>> PatchWithResponseCode<TData, TResponse>(IPatchApiRequest<TData> request,
        bool includeResponse = true)
    {
        throw new System.NotImplementedException();
    }

    public Task<ApiResponse<TResponse>> PutWithResponseCode<TData, TResponse>(IPutApiRequest<TData> request)
    {
        throw new System.NotImplementedException();
    }
}
