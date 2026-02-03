using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Services;

public class EmploymentCheckApiClient(IInternalApiClient<EmploymentCheckConfiguration> apiClient)
    : IEmploymentCheckApiClient<EmploymentCheckConfiguration>
{
    public Task<TResponse> Get<TResponse>(IGetApiRequest request) =>
        apiClient.Get<TResponse>(request);

    public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request) =>
        apiClient.GetAll<TResponse>(request);

    public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request) =>
        apiClient.GetResponseCode(request);

    public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request) =>
        apiClient.GetWithResponseCode<TResponse>(request);

    public Task<TResponse> Post<TResponse>(IPostApiRequest request) =>
        apiClient.Post<TResponse>(request);

    public Task Post<TData>(IPostApiRequest<TData> request) =>
        apiClient.Post(request);

    public Task Delete(IDeleteApiRequest request) =>
        apiClient.Delete(request);

    public Task<ApiResponse<TResponse>> DeleteWithResponseCode<TResponse>(IDeleteApiRequest request, bool includeResponse = false) =>
        apiClient.DeleteWithResponseCode<TResponse>(request, includeResponse);

    public Task Patch<TData>(IPatchApiRequest<TData> request) =>
        apiClient.Patch(request);

    public Task Put(IPutApiRequest request) =>
        apiClient.Put(request);

    public Task Put<TData>(IPutApiRequest<TData> request) =>
        apiClient.Put(request);

    public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true) =>
        apiClient.PostWithResponseCode<TResponse>(request, includeResponse);

    public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request) =>
        apiClient.PatchWithResponseCode(request);

    public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request) =>
        apiClient.GetPaged<TResponse>(request);

    public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request) where TResponse : class =>
        apiClient.PutWithResponseCode<TResponse>(request);

    public Task<ApiResponse<TResponse>> PatchWithResponseCode<TData, TResponse>(IPatchApiRequest<TData> request, bool includeResponse = true) =>
        apiClient.PatchWithResponseCode<TData, TResponse>(request, includeResponse);

    public Task<ApiResponse<TResponse>> PutWithResponseCode<TData, TResponse>(IPutApiRequest<TData> request) =>
        apiClient.PutWithResponseCode<TData, TResponse>(request);
}
