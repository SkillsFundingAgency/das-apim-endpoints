using SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface ICustomerEngagementFinanceService
    {
        Task<GetVendorByApprenticeshipLegalEntityIdResponse> GetVendorByApprenticeshipLegalEntityId(string companyName, string hashedLegalEntityId);
        Task<GetVendorRegistrationStatusByCaseIdResponse> GetVendorRegistrationStatusByCaseId(string caseId);
        Task<GetVendorRegistrationCaseStatusUpdateResponse> GetVendorRegistrationCasesByLastStatusChangeDate(DateTime dateTimeFrom, DateTime dateTimeTo);
    }
}