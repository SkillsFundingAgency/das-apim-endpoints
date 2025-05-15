using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Services;

public class EarningsApiClient : IEarningsApiClient<EarningsApiConfiguration>
{
    private readonly IInternalApiClient<EarningsApiConfiguration> _apiClient;

    public EarningsApiClient(IInternalApiClient<EarningsApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
    {
        return await _apiClient.GetAll<TResponse>(request);
    }

    public async Task<TResponse> Get<TResponse>(IGetApiRequest request)
    {
        var result = await _apiClient.GetWithResponseCode<TResponse>(request);
        if (result.StatusCode == HttpStatusCode.Unauthorized) throw new ApiUnauthorizedException();
        return result.Body;
    }

    public async Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
    {
        return await _apiClient.GetResponseCode(request);
    }

    public async Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
    {
        return await _apiClient.GetWithResponseCode<TResponse>(request);
    }

    public async Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
    {
        return await _apiClient.GetPaged<TResponse>(request);
    }

    public async Task<TResponse> Post<TResponse>(IPostApiRequest request)
    {
        return (await _apiClient.PostWithResponseCode<TResponse>(request, true)).Body;
    }

    public Task Post<TData>(IPostApiRequest<TData> request)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(IDeleteApiRequest request)
    {
        await _apiClient.Delete(request);
    }

    public Task<ApiResponse<TResponse>> DeleteWithResponseCode<TResponse>(IDeleteApiRequest request, bool includeResponse = false)
    {
        return _apiClient.DeleteWithResponseCode<TResponse>(request, includeResponse);
    }

    public async Task Patch<TData>(IPatchApiRequest<TData> request)
    {
        await _apiClient.Patch(request);
    }

    public async Task Put(IPutApiRequest request)
    {
        await _apiClient.Put(request);
    }

    public async Task Put<TData>(IPutApiRequest<TData> request)
    {
        await _apiClient.Put(request);
    }

    public async Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
    {
        return await _apiClient.PostWithResponseCode<TResponse>(request, includeResponse);
    }

    public async Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
    {
        return await _apiClient.PatchWithResponseCode(request);
    }

    public async Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request)
    {
        return await _apiClient.PutWithResponseCode<TResponse>(request);
    }

    public Task<ApiResponse<TResponse>> PatchWithResponseCode<TData, TResponse>(IPatchApiRequest<TData> request, bool includeResponse = true)
    {
        throw new NotImplementedException();
    }
}