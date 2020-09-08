using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Services
{
    public class LocationApiClient : ILocationApiClient<LocationApiConfiguration>
    {
        private readonly IInternalApiClient<LocationApiConfiguration> _client;

        public LocationApiClient (IInternalApiClient<LocationApiConfiguration> client)
        {
            _client = client;
        }
        public Task<TResponse> Get<TResponse>(IGetApiRequest request, bool ensureSuccessResponseCode = true)
        {
            return _client.Get<TResponse>(request, ensureSuccessResponseCode);
        }

        public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            return _client.GetResponseCode(request);
        }

        public Task<TResponse> Post<TResponse>(IPostApiRequest request)
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
            throw new System.NotImplementedException();
        }

        public Task Put<TData>(IPutApiRequest<TData> request)
        {
            throw new System.NotImplementedException();
        }
    }
}