﻿using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using Accounts = SFA.DAS.EmployerIncentives.InnerApi.Responses.Accounts;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerIncentives.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IEmployerIncentivesService
    {
        Task<bool> IsHealthy();
        Task<ApprenticeshipItem[]> GetEligibleApprenticeships(IEnumerable<ApprenticeshipItem> allApprenticeship);
        Task<AccountLegalEntity[]> GetAccountLegalEntities(long accountId);
        Task<AccountLegalEntity> GetLegalEntity(long accountId, long accountLegalEntityId);
        Task DeleteAccountLegalEntity(long accountId, long accountLegalEntityId);
        Task ConfirmIncentiveApplication(ConfirmIncentiveApplicationRequest request, CancellationToken cancellationToken = default);
        Task CreateLegalEntity(long accountId, AccountLegalEntityCreateRequest accountLegalEntity);
        Task SendBankDetailRequiredEmail(long accountId, SendBankDetailsEmailRequest sendBankDetailsEmailRequest);
        Task SendBankDetailReminderEmail(long accountId, SendBankDetailsEmailRequest sendBankDetailsEmailRequest);
        Task<IncentiveApplicationDto> GetApplication(long accountId, Guid applicationId);
        Task CreateIncentiveApplication(CreateIncentiveApplicationRequestData requestData);
        Task UpdateIncentiveApplication(UpdateIncentiveApplicationRequestData requestData);
        Task<long> GetApplicationLegalEntity(long accountId, Guid applicationId);
        Task SignAgreement(long accountId, long accountLegalEntityId, SignAgreementRequest request);
        Task<GetIncentiveDetailsResponse> GetIncentiveDetails();
        Task UpdateVendorRegistrationCaseStatus(UpdateVendorRegistrationCaseStatusRequest request);
        Task<GetApplicationsResponse> GetApprenticeApplications(long accountId, long accountLegalEntityId);
        Task AddEmployerVendorIdToLegalEntity(string hashedLegalEntityId, string employerVendorId);
        Task EarningsResilienceCheck();
        Task<GetLatestVendorRegistrationCaseUpdateDateTimeResponse> GetLatestVendorRegistrationCaseUpdateDateTime();
        Task SendBankDetailsRepeatReminderEmails(SendBankDetailsRepeatReminderEmailsRequest sendBankDetailsRepeatReminderEmailsRequest);

        Task UpdateCollectionCalendarPeriod(UpdateCollectionCalendarPeriodRequestData requestData);
        Task RefreshLegalEntities(IEnumerable<Accounts.AccountLegalEntity> accountLegalEntities, int pageNumber, int pageSize, int totalPages);
    }
}