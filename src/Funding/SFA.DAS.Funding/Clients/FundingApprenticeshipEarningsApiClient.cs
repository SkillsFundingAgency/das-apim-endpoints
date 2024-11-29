using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.Funding.Configuration;
using SFA.DAS.Funding.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Funding.Clients
{
    public class FundingApprenticeshipEarningsApiClient : IFundingApprenticeshipEarningsApiClient<FundingApprenticeshipEarningsConfiguration>
    {
        private readonly IInternalApiClient<FundingApprenticeshipEarningsConfiguration> _client;

        public FundingApprenticeshipEarningsApiClient(IInternalApiClient<FundingApprenticeshipEarningsConfiguration> client)
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
            return _client.PostWithResponseCode<TResponse>(request, includeResponse);
        }

        public Task<ApiResponse<string>> PatchWithResponseCode<TData>(IPatchApiRequest<TData> request)
        {
            return _client.PatchWithResponseCode(request);
        }

        public Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request)
        {
            return _client.PutWithResponseCode<TResponse>(request);
        }

        public Task<PagedResponse<TResponse>> GetPaged<TResponse>(IGetPagedApiRequest request)
        {
            return _client.GetPaged<TResponse>(request);
        }

        public Task<ApiResponse<TResponse>> PatchWithResponseCode<TData, TResponse>(IPatchApiRequest<TData> request, bool includeResponse = true)
        {
            throw new System.NotImplementedException();
        }
    }
}