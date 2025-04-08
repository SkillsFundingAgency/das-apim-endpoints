using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Services;

[ExcludeFromCodeCoverage]
public class AccessorServiceInnerApiClient : IAccessorServiceInnerApiClient<AccessorServiceInnerApiConfiguration>
{
    private readonly IInternalApiClient<AccessorServiceInnerApiConfiguration> _client;

    public AccessorServiceInnerApiClient(IInternalApiClient<AccessorServiceInnerApiConfiguration> client)
    {
        _client = client;
    }
    public Task<TResponse> Get<TResponse>(IGetApiRequest request)
    {
        return _client.Get<TResponse>(request);
    }

    public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
    {
        throw new System.NotImplementedException();
    }

    public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
    {
        return _client.GetResponseCode(request);
    }

    public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
    {
        return _client.GetWithResponseCode<TResponse>(request);
    }

    public Task<TResponse> Post<TResponse>(IPostApiRequest request)
    {
        throw new System.NotImplementedException();
    }

    public Task Delete(IDeleteApiRequest request)
    {
        throw new System.NotImplementedException();
    }

    public Task Patch<TData>(IPatchApiRequest<TData> request)
    {
        throw new System.NotImplementedException();
    }

    public Task Put(IPutApiRequest request)
    {
        throw new System.NotImplementedException();
    }

    public Task Put<TData>(IPutApiRequest<TData> request)
    {
        throw new System.NotImplementedException();
    }

    public async Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
    {
        return await _client.PostWithResponseCode<TResponse>(request, includeResponse);
    }

    public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
    {
        throw new System.NotImplementedException();
    }

    public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
    {
        throw new System.NotImplementedException();
    }
    public Task Post<TData>(IPostApiRequest<TData> request)

    {
        throw new System.NotImplementedException();
    }
    public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request)
    {
        throw new System.NotImplementedException();
    }

    public Task<ApiResponse<TResponse>> PatchWithResponseCode<TData, TResponse>(IPatchApiRequest<TData> request, bool includeResponse = true)
    {
        throw new System.NotImplementedException();
    }
}
