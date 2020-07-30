using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.Services
{
    public class EmployerIncentivesApiClient : IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>
    {
        private readonly IApiClient<EmployerIncentivesConfiguration> _client;

        public EmployerIncentivesApiClient (IApiClient<EmployerIncentivesConfiguration> client)
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
    }
}