using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Services
{
    [ExcludeFromCodeCoverage]
    public class RoatpCourseManagementApiClient : IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>
    {
        private readonly IInternalApiClient<RoatpV2ApiConfiguration> _apiClient;

        public RoatpCourseManagementApiClient(IInternalApiClient<RoatpV2ApiConfiguration> apiClient)
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

        public async Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request)
        {
            return await _apiClient.PostWithResponseCode<TResponse>(request);
        }

        public async Task Delete(IDeleteApiRequest request)
        {
            await _apiClient.Delete(request);
        }

        public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task Patch<TData>(IPatchApiRequest<TData> request)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
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

        public Task Put(IPutApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task Put<TData>(IPutApiRequest<TData> request)
        {
            throw new System.NotImplementedException();
        }
    }
}
