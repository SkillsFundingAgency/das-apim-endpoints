using System;
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
    public class ApprenticeshipsApiClient : IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>
    {
        private readonly ITokenPassThroughInternalApiClient<ApprenticeshipsApiConfiguration> _apiClient;

        public ApprenticeshipsApiClient(ITokenPassThroughInternalApiClient<ApprenticeshipsApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            return await _apiClient.GetAll<TResponse>(request);
        }

        public async Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            return await _apiClient.Get<TResponse>(request);
        }

        public async Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            return await _apiClient.GetResponseCode(request);
        }

        public async Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            return await _apiClient.GetWithResponseCode<TResponse>(request);
        }

        public async Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
        {
            return await _apiClient.GetPaged<TResponse>(request);
        }

        public async Task<TResponse> Post<TResponse>(IPostApiRequest request)
        {
            return (await _apiClient.PostWithResponseCode<TResponse>(request, true)).Body;
        }

        public Task Post<TData>(IPostApiRequest<TData> request)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(IDeleteApiRequest request)
        {
            await _apiClient.Delete(request);
        }

        public async Task Patch<TData>(IPatchApiRequest<TData> request)
        {
            await _apiClient.Patch(request);
        }

        public async Task Put(IPutApiRequest request)
        {
            await _apiClient.Put(request);
        }

        public async Task Put<TData>(IPutApiRequest<TData> request)
        {
            await _apiClient.Put(request);
        }

        public async Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
        {
            return await _apiClient.PostWithResponseCode<TResponse>(request, includeResponse);
        }

        public async Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
        {
            return await _apiClient.PatchWithResponseCode(request);
        }

        public async Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request)
        {
            return await _apiClient.PutWithResponseCode<TResponse>(request);
        }
    }
}