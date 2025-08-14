using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerIncentives.Clients
{
    public class CommitmentsApiClient : ICommitmentsApiClient<CommitmentsConfiguration>
    {
        private readonly IInternalApiClient<CommitmentsConfiguration> _client;

        public CommitmentsApiClient(IInternalApiClient<CommitmentsConfiguration> client)
        {
            _client = client;
        }
        public Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            return _client.Get<TResponse>(request);
        }

        public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            return _client.GetAll<TResponse>(request);
        }

        public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            return _client.GetResponseCode(request);
        }

        public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            return _client.GetWithResponseCode<TResponse>(request);
        }

        public Task<TResponse> Post<TResponse>(IPostApiRequest request)
        {
            return _client.Post<TResponse>(request);
        }

        public Task Post<TData>(IPostApiRequest<TData> request)
        {
            return _client.Post(request);
        }

        public Task Delete(IDeleteApiRequest request)
        {
            return _client.Delete(request);
        }

        public Task Patch<TData>(IPatchApiRequest<TData> request)
        {
            return _client.Patch(request);
        }

        public Task Put(IPutApiRequest request)
        {
            return _client.Put(request);
        }

        public Task Put<TData>(IPutApiRequest<TData> request)
        {
            return _client.Put(request);
        }

        public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
        {
            return _client.GetPaged<TResponse>(request);
        }

        public Task<ApiResponse<TResponse>> PatchWithResponseCode<TData, TResponse>(IPatchApiRequest<TData> request, bool includeResponse = true)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse<TResponse>> DeleteWithResponseCode<TResponse>(IDeleteApiRequest request, bool includeResponse = false)
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