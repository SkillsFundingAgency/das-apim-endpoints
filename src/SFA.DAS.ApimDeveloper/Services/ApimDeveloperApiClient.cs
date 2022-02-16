using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.ApimDeveloper.Services
{
    public class ApimDeveloperApiClient : IApimDeveloperApiClient<ApimDeveloperApiConfiguration>
    {
        private readonly IInternalApiClient<ApimDeveloperApiConfiguration> _apiClient;

        public ApimDeveloperApiClient (IInternalApiClient<ApimDeveloperApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        
        public Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            return _apiClient.Get<TResponse>(request);
        }

        public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            return _apiClient.GetWithResponseCode<TResponse>(request);
        }

        public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
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
            return _apiClient.Put(request);
        }

        public Task Put<TData>(IPutApiRequest<TData> request)
        {
            return _apiClient.Put(request);
        }

        public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request)
        {
            return _apiClient.PostWithResponseCode<TResponse>(request);
        }

        public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
        {
            throw new System.NotImplementedException();
        }
    }
}
            