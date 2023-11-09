using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Services
{
    public class ApprenticeshipsApiClient : IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>
    {
        private readonly IInternalApiClient<ApprenticeshipsApiConfiguration> _apiClient;

        public ApprenticeshipsApiClient(IInternalApiClient<ApprenticeshipsApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            return _apiClient.GetWithResponseCode<TResponse>(request);
        }

        public async Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<TResponse> Post<TResponse>(IPostApiRequest request)
        {
            return _apiClient.Post<TResponse>(request);
        }

        public Task Post<TData>(IPostApiRequest<TData> request)
        {
            return _apiClient.Post(request);
        }

        public async Task Delete(IDeleteApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public async Task Patch<TData>(IPatchApiRequest<TData> request)
        {
            throw new System.NotImplementedException();
        }

        public async Task Put(IPutApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public async Task Put<TData>(IPutApiRequest<TData> request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}