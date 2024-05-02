using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Services;
public class RecruitApiClient : IRecruitApiClient<RecruitApiConfiguration>
{
    private readonly IInternalApiClient<RecruitApiConfiguration> _apiClient;

    public RecruitApiClient(IInternalApiClient<RecruitApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
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

    public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
    {
        return _apiClient.PostWithResponseCode<TResponse>(request, includeResponse);
    }

    public Task Delete(IDeleteApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task Patch<TData>(IPatchApiRequest<TData> request)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
    {
        throw new NotImplementedException();
    }

    public Task<TResponse> Post<TResponse>(IPostApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task Post<TData>(IPostApiRequest<TData> request)
    {
        throw new NotImplementedException();
    }

    public Task Put(IPutApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task Put<TData>(IPutApiRequest<TData> request)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request)
    {
        throw new NotImplementedException();
    }
}
