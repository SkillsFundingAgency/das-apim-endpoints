using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Services
{
    [ExcludeFromCodeCoverage]
    public class ShortlistApiClient : IShortlistApiClient<ShortlistApiConfiguration>
    {
        private readonly IInternalApiClient<ShortlistApiConfiguration> _apiClient;

        public ShortlistApiClient(IInternalApiClient<ShortlistApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public Task Delete(IDeleteApiRequest request)
        {
            return _apiClient.Delete(request);
        }

        public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
        {
            return _apiClient.PostWithResponseCode<TResponse>(request, includeResponse);
        }

        public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            return _apiClient.GetAll<TResponse>(request);
        }

        public Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            return _apiClient.Get<TResponse>(request);
        }

        public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            throw new NotImplementedException();
        }

        public Task Patch<TData>(IPatchApiRequest<TData> request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> Post<TResponse>(IPostApiRequest request)
        {
            throw new NotImplementedException();
        }

        public Task Post<TData>(IPostApiRequest<TData> request)
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
    }
}
