using System.Collections.Generic;
using System.Net;
using SFA.DAS.EmploymentCheck.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmploymentCheck.Clients
{
    public class EmploymentCheckApiClient : IEmploymentCheckApiClient<EmploymentCheckConfiguration>
    {
        private readonly IInternalApiClient<EmploymentCheckConfiguration> _client;

        public EmploymentCheckApiClient(IInternalApiClient<EmploymentCheckConfiguration> client)
        {
            _client = client;
        }

        public Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            return _client.Get<TResponse>(request);
        }

        public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            throw new System.NotImplementedException();
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
            return _client.Put(request);
        }

        public Task Put<TData>(IPutApiRequest<TData> request)
        {
            return _client.Put(request);
        }

        public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
        {
            throw new System.NotImplementedException();
        }

        public Task<HttpStatusCode> GetResponseCode(GetPingRequest getPingRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}