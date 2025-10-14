using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Services;

[ExcludeFromCodeCoverage]

public class RoatpServiceApiClient : IRoatpServiceApiClient<RoatpConfiguration>
{
    private readonly IInternalApiClient<RoatpConfiguration> _apiClient;

    public RoatpServiceApiClient(IInternalApiClient<RoatpConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public Task Delete(IDeleteApiRequest request)
    {
        throw new System.NotImplementedException();
    }

    public Task<ApiResponse<TResponse>> DeleteWithResponseCode<TResponse>(IDeleteApiRequest request, bool includeResponse = false)
    {
        throw new System.NotImplementedException();
    }

    public Task<TResponse> Get<TResponse>(IGetApiRequest request)
    {
        return _apiClient.Get<TResponse>(request);
    }

    public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
    {
        throw new System.NotImplementedException();
    }

    public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
    {
        throw new System.NotImplementedException();
    }

    public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
    {
        return _apiClient.GetResponseCode(request);
    }

    public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
    {
        return _apiClient.GetWithResponseCode<TResponse>(request);
    }

    public Task Patch<TData>(IPatchApiRequest<TData> request)
    {
        throw new System.NotImplementedException();
    }

    public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
    {
        throw new System.NotImplementedException();
    }

    public Task<ApiResponse<TResponse>> PatchWithResponseCode<TData, TResponse>(IPatchApiRequest<TData> request, bool includeResponse = true)
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

    public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
    {
        return _apiClient.PostWithResponseCode<TResponse>(request);
    }

    public Task Put(IPutApiRequest request)
    {
        return _apiClient.Put(request);
    }

    public Task Put<TData>(IPutApiRequest<TData> request)
    {
        return _apiClient.Put(request);
    }

    public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request) where TResponse : class
    {
        return _apiClient.PutWithResponseCode<TResponse>(request);
    }

    public Task<ApiResponse<TResponse>> PutWithResponseCode<TData, TResponse>(IPutApiRequest<TData> request)
    {
        return _apiClient.PutWithResponseCode<TData, TResponse>(request);
    }
}