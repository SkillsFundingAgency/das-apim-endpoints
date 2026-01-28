using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;


namespace SFA.DAS.SharedOuterApi.Services
{
    public class EmployerFeedbackApiClient : IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>
    {
        private readonly IInternalApiClient<EmployerFeedbackApiConfiguration> _apiClient;

        public EmployerFeedbackApiClient(IInternalApiClient<EmployerFeedbackApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            return _apiClient.Get<TResponse>(request);
        }

        public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            return _apiClient.GetAll<TResponse>(request);
        }

        public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
        {
            return _apiClient.GetPaged<TResponse>(request);
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

        public Task<ApiResponse<TResponse>> PostWithResponseCode<TData, TResponse>(IPostApiRequest<TData> request, bool includeResponse = true)
        {
            return _apiClient.PostWithResponseCode<TData, TResponse>(request, includeResponse);
        }

        public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
        {
            return _apiClient.PatchWithResponseCode(request);
        }

        public Task Put(IPutApiRequest request)
        {
            return _apiClient.Put(request);
        }

        public Task Put<TData>(IPutApiRequest<TData> request)
        {
            return _apiClient.Put(request);
        }

        public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request) where TResponse : class
        {
            return _apiClient.PutWithResponseCode<TResponse>(request);
        }

        public Task Delete(IDeleteApiRequest request)
        {
            return _apiClient.Delete(request);
        }

        public Task<ApiResponse<TResponse>> DeleteWithResponseCode<TResponse>(IDeleteApiRequest request, bool includeResponse = false)
        {
            return _apiClient.DeleteWithResponseCode<TResponse>(request, includeResponse);
        }

        // Methods Now Obsolete.
        public Task<TResponse> Post<TResponse>(IPostApiRequest request)
        {
            throw new NotImplementedException();
        }

        public Task Post<TData>(IPostApiRequest<TData> request)
        {
            throw new NotImplementedException();
        }

        public Task Patch<TData>(IPatchApiRequest<TData> request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<TResponse>> PatchWithResponseCode<TData, TResponse>(IPatchApiRequest<TData> request, bool includeResponse = true)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<TResponse>> PutWithResponseCode<TData, TResponse>(IPutApiRequest<TData> request)
        {
            return _apiClient.PutWithResponseCode<TData, TResponse>(request);
        }
    }
}
