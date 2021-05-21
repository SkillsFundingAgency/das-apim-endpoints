using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IEmployerIncentivesService
    {
        Task<bool> IsHealthy();
        Task<ApprenticeshipItem[]> GetEligibleApprenticeships(IEnumerable<ApprenticeshipItem> allApprenticeship);
        Task SendBankDetailRequiredEmail(long accountId, SendBankDetailsEmailRequest sendBankDetailsEmailRequest);
        Task SendBankDetailReminderEmail(long accountId, SendBankDetailsEmailRequest sendBankDetailsEmailRequest);
        Task SignAgreement(long accountId, long accountLegalEntityId, SignAgreementRequest request);
        Task<GetIncentiveDetailsResponse> GetIncentiveDetails();
        Task UpdateVendorRegistrationCaseStatus(UpdateVendorRegistrationCaseStatusRequest request);
        Task AddEmployerVendorIdToLegalEntity(string hashedLegalEntityId, string employerVendorId);
        Task EarningsResilienceCheck();
        Task<GetLatestVendorRegistrationCaseUpdateDateTimeResponse> GetLatestVendorRegistrationCaseUpdateDateTime();
        Task SendBankDetailsRepeatReminderEmails(SendBankDetailsRepeatReminderEmailsRequest sendBankDetailsRepeatReminderEmailsRequest);
        Task UpdateCollectionCalendarPeriod(UpdateCollectionCalendarPeriodRequestData requestData);
    }
}