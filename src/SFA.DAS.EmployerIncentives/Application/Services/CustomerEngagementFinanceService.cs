using Microsoft.Extensions.Options;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class CustomerEngagementFinanceService : ICustomerEngagementFinanceService
    {
        private readonly ICustomerEngagementFinanceApiClient<CustomerEngagementFinanceConfiguration> _client;
        private readonly ICustomerEngagementApiConfiguration _config;

        public CustomerEngagementFinanceService(
            ICustomerEngagementFinanceApiClient<CustomerEngagementFinanceConfiguration> client,
            IOptions<CustomerEngagementFinanceConfiguration> configOptions)
        {
            _client = client;
            _config = configOptions.Value;
        }

        public async Task<GetVendorRegistrationCaseStatusUpdateResponse> GetVendorRegistrationCasesByLastStatusChangeDate(DateTime dateTimeFrom, DateTime dateTimeTo, string skipCode = null)
        {
            return await _client.Get<GetVendorRegistrationCaseStatusUpdateResponse>(new GetVendorRegistrationStatusByLastStatusChangeDateRequest(dateTimeFrom, dateTimeTo, skipCode));
        }

        public Task<GetVendorByApprenticeshipLegalEntityIdResponse> GetVendorByApprenticeshipLegalEntityId(string hashedLegalEntityId)
        {            
            return _client.Get<GetVendorByApprenticeshipLegalEntityIdResponse>(new GetVendorByApprenticeshipLegalEntityId(_config.CompanyName, hashedLegalEntityId, _config.ApiVersion));
        }
    }
}
