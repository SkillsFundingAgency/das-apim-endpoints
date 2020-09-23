using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;
using System;
using System.Threading.Tasks;

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

        public async Task<GetVendorRegistrationStatusByCaseIdResponse> GetVendorRegistrationStatusByCaseId(string caseId)
        {
            var response = await _client.Get<GetVendorRegistrationStatusByCaseIdResponse>(new GetVendorRegistrationStatusByCaseIdRequest(caseId), false);

            return response;
        }

        public async Task<GetVendorRegistrationCaseStatusUpdateResponse> GetVendorRegistrationCasesByLastStatusChangeDate(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            var response = await _client.Get<GetVendorRegistrationCaseStatusUpdateResponse>(new GetVendorRegistrationStatusByLastStatusChangeDateRequest(dateTimeFrom, dateTimeTo), false);

            return response;
        }

    }
}
