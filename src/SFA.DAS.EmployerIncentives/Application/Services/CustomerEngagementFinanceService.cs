using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.CustomerEngagementFinance;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;
using System;
using System.Net;
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

        public async Task<GetVendorRegistrationCaseStatusUpdateResponse> GetVendorRegistrationCasesByLastStatusChangeDate(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            var response = await _client.Get<GetVendorRegistrationCaseStatusUpdateResponse>(new GetVendorRegistrationStatusByLastStatusChangeDateRequest(dateTimeFrom, dateTimeTo), false);

            return response;
        }

        public async Task<bool> IsHealthy()
        {
            try
            {
                var status = await _client.GetResponseCode(new GetCustomerEngagementFinanceHeartbeatRequest());
                return status == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }
    }
}
