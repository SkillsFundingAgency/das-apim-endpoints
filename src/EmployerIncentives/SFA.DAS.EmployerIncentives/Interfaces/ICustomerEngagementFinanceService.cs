using SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface ICustomerEngagementFinanceService
    {
        Task<GetVendorRegistrationCaseStatusUpdateResponse> GetVendorRegistrationCasesByLastStatusChangeDate(DateTime dateTimeFrom, DateTime dateTimeTo, string skipCode = null);
        Task<GetVendorByApprenticeshipLegalEntityIdResponse> GetVendorByApprenticeshipLegalEntityId(string hashedLegalEntityId);
    }
}