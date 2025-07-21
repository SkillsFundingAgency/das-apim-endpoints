using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Services
{
    public class ProviderPaymentEventsApiClient : IProviderPaymentEventsApiClient<ProviderEventsConfiguration>
    {
        private readonly IInternalApiClient<ProviderEventsConfiguration> _apiClient;

        public ProviderPaymentEventsApiClient(IInternalApiClient<ProviderEventsConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public Task Delete(IDeleteApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse<TResponse>> DeleteWithResponseCode<TResponse>(IDeleteApiRequest request, bool includeResponse = false)
        {
            return _apiClient.DeleteWithResponseCode<TResponse>(request, includeResponse);
        }

        public Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            return _apiClient.Get<TResponse>(request);
        }

        public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            return _apiClient.GetResponseCode(request);
        }

        public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            return _apiClient.GetWithResponseCode<TResponse>(request);
        }

        public Task Patch<TData>(IPatchApiRequest<TData> request)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse<TResponse>> PatchWithResponseCode<TData, TResponse>(IPatchApiRequest<TData> request, bool includeResponse = true)
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

        public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
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

        public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request) where TResponse : class
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse<TResponse>> PutWithResponseCode<TData, TResponse>(IPutApiRequest<TData> request)
        {
            throw new System.NotImplementedException();
        }
    }
}
