using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class CustomerEngagementFinanceService : ICustomerEngagementFinanceService
    {
        private readonly ICustomerEngagementFinanceApiClient<CustomerEngagementFinanceConfiguration> _client;

        public CustomerEngagementFinanceService(ICustomerEngagementFinanceApiClient<CustomerEngagementFinanceConfiguration> client)
        {
            _client = client;
        }

        public async Task<GetVendorByApprenticeshipLegalEntityIdResponse> GetVendorByApprenticeshipLegalEntityId(string companyName, string hashedLegalEntityId)
        {
            var response = await _client.Get<GetVendorByApprenticeshipLegalEntityIdResponse>(new GetVendorByApprenticeshipLegalEntityId(companyName, hashedLegalEntityId), false);

            return response;
        }
    }
}
