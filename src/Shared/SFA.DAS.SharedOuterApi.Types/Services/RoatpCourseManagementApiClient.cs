using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace SFA.DAS.SharedOuterApi.Types.Services;

[ExcludeFromCodeCoverage]
public class RoatpCourseManagementApiClient(IInternalApiClient<RoatpV2ApiConfiguration> apiClient)
    : IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>
{
    public Task<TResponse> Get<TResponse>(IGetApiRequest request)
    {
        return apiClient.Get<TResponse>(request);
    }

    public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
    {
        return apiClient.GetResponseCode(request);
    }

    public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
    {
        return apiClient.GetWithResponseCode<TResponse>(request);
    }

    public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
    {
        return apiClient.PostWithResponseCode<TResponse>(request, includeResponse);
    }

    public async Task Delete(IDeleteApiRequest request)
    {
        await apiClient.Delete(request);
    }

    public Task<ApiResponse<TResponse>> DeleteWithResponseCode<TResponse>(IDeleteApiRequest request, bool includeResponse = false)
    {
        return apiClient.DeleteWithResponseCode<TResponse>(request, includeResponse);
    }

    public async Task Put<TData>(IPutApiRequest<TData> request)
    {
        await apiClient.Put(request);
    }

    public Task Put(IPutApiRequest request)
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
        return apiClient.PatchWithResponseCode(request);
    }

    public Task<TResponse> Post<TResponse>(IPostApiRequest request)
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
