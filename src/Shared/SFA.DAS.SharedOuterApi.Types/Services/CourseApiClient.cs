using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;

namespace SFA.DAS.SharedOuterApi.Types.Services;

public class CourseApiClient(IInternalApiClient<CoursesApiConfiguration> apiClient)
    : ICoursesApiClient<CoursesApiConfiguration>
{
    public Task<TResponse> Get<TResponse>(IGetApiRequest request)
    {
        return apiClient.Get<TResponse>(request);
    }

    public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
    {
        return apiClient.GetAll<TResponse>(request);
    }

    public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
    {
        return apiClient.GetResponseCode(request);
    }

    public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
    {
        return apiClient.GetWithResponseCode<TResponse>(request);
    }

    public Task<TResponse> Post<TResponse>(IPostApiRequest request)
    {
        return apiClient.Post<TResponse>(request);
    }

    public Task Post<TData>(IPostApiRequest<TData> request)
    {
        return apiClient.Post(request);
    }

    public Task Delete(IDeleteApiRequest request)
    {
        return apiClient.Delete(request);
    }

    public Task<ApiResponse<TResponse>> DeleteWithResponseCode<TResponse>(IDeleteApiRequest request, bool includeResponse = false)
    {
        return apiClient.DeleteWithResponseCode<TResponse>(request, includeResponse);
    }

    public Task Patch<TData>(IPatchApiRequest<TData> request)
    {
        return apiClient.Patch(request);
    }

    public Task Put(IPutApiRequest request)
    {
        return apiClient.Put(request);
    }

    public Task Put<TData>(IPutApiRequest<TData> request)
    {
        return apiClient.Put(request);
    }
    public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request) where TResponse : class
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
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