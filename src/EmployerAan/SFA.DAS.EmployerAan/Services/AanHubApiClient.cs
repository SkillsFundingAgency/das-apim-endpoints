using System.Diagnostics.CodeAnalysis;
using System.Net;
using SFA.DAS.EmployerAan.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerAan.Services;

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
