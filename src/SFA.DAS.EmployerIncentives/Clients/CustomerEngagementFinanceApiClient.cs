using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Clients
{
    public class CustomerEngagementFinanceApiClient : ICustomerEngagementFinanceApiClient<CustomerEngagementFinanceConfiguration>
    {
        private readonly ICustomerEngagementApiClient<CustomerEngagementFinanceConfiguration> _client;

        public CustomerEngagementFinanceApiClient(ICustomerEngagementApiClient<CustomerEngagementFinanceConfiguration> client)
        {
            _client = client;
        }

        public Task<TResponse> Get<TResponse>(IGetApiRequest request, bool ensureSuccessResponseCode = true)
        {
            return _client.Get<TResponse>(request, ensureSuccessResponseCode);
        }
    }
}