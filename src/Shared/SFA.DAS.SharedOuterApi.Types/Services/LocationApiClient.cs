using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;

namespace SFA.DAS.SharedOuterApi.Types.Services;

public class LocationApiClient(IInternalApiClient<LocationApiConfiguration> client)
    : ILocationApiClient<LocationApiConfiguration>
{
    public Task<TResponse> Get<TResponse>(IGetApiRequest request)
    {
        return client.Get<TResponse>(request);
    }

    public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
    {
        return client.GetResponseCode(request);
    }

    public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
    {
        return client.GetWithResponseCode<TResponse>(request);
    }

    public Task<TResponse> Post<TResponse>(IPostApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<TResponse>> DeleteWithResponseCode<TResponse>(IDeleteApiRequest request, bool includeResponse = false)
    {
        return client.DeleteWithResponseCode<TResponse>(request, includeResponse);
    }


    public Task Delete(IDeleteApiRequest request)
    {
        throw new NotImplementedException();
    }

    public Task Patch<TData>(IPatchApiRequest<TData> request)
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

    public async Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
    {
        return await client.PostWithResponseCode<TResponse>(request, includeResponse);
    }

    public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
    {
        throw new NotImplementedException();
    }
    public Task Post<TData>(IPostApiRequest<TData> request)

    {
        throw new NotImplementedException();
    }
    public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request) where TResponse : class
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<TResponse>> PatchWithResponseCode<TData, TResponse>(IPatchApiRequest<TData> request, bool includeResponse = true)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<TResponse>> PutWithResponseCode<TData, TResponse>(IPutApiRequest<TData> request)
    {
        throw new NotImplementedException();
    }
}