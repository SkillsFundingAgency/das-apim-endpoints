using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerIncentives.Clients
{
    public class CustomerEngagementFinanceApiClient : ICustomerEngagementFinanceApiClient<CustomerEngagementFinanceConfiguration>
    {
        private readonly ICustomerEngagementApiClient<CustomerEngagementFinanceConfiguration> _client;

        public CustomerEngagementFinanceApiClient(ICustomerEngagementApiClient<CustomerEngagementFinanceConfiguration> client)
        {
            _client = client;
        }

        public Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            return _client.Get<TResponse>(request);
        }

        public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            return _client.GetResponseCode(request);
        }

        public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            return _client.GetWithResponseCode<TResponse>(request);
        }
    }
}