using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Services
{
    [ExcludeFromCodeCoverage]
    public class RequestApprenticeTrainingApiClient : IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>
    {
        private readonly IInternalApiClient<RequestApprenticeTrainingApiConfiguration> _apiClient;

        public RequestApprenticeTrainingApiClient(IInternalApiClient<RequestApprenticeTrainingApiConfiguration> apiClient)
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
        public Task Delete(IDeleteApiRequest request)
        {
            return _apiClient.Delete(request);
        }
        public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request)
        {
            throw new NotImplementedException();
        }
        
        // Method Obsolete
        public Task<TResponse> Post<TResponse>(IPostApiRequest request)
        {
            throw new NotImplementedException();
        }

        // Method Obsolete
        public Task Post<TData>(IPostApiRequest<TData> request)
        {
            throw new NotImplementedException();
        }

        public Task Patch<TData>(IPatchApiRequest<TData> request)
        {
            throw new NotImplementedException();
        }
    }
}
