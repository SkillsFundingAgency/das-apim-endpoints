using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.EarningsResilienceCheck;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using Accounts = SFA.DAS.EmployerIncentives.InnerApi.Responses.Accounts;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Exceptions;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class EmployerIncentivesService : IEmployerIncentivesService
    {
        private readonly IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> _client;

        public EmployerIncentivesService(IEmployerIncentivesApiClient<EmployerIncentivesConfiguration> client)
        {
            _client = client;
        }

        public async Task<bool> IsHealthy()
        {
            try
            {
                var status = await _client.GetResponseCode(new GetHealthRequest());
                return (status == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
        }

        public async Task<ApprenticeshipItem[]> GetEligibleApprenticeships(IEnumerable<ApprenticeshipItem> allApprenticeship)
        {
            var bag = new ConcurrentBag<ApprenticeshipItem>();
            var tasks = allApprenticeship.Select(x => VerifyApprenticeshipIsEligible(x, bag));
            await Task.WhenAll(tasks);

            return bag.ToArray();
        }

        public async Task<AccountLegalEntity[]> GetAccountLegalEntities(long accountId)
        {
            var response = await _client.GetAll<AccountLegalEntity>(new GetAccountLegalEntitiesRequest(accountId));

            return response.ToArray();
        }

        public async Task<AccountLegalEntity> GetLegalEntity(long accountId, long accountLegalEntityId)
        {
            var response = await _client.Get<AccountLegalEntity>(new InnerApi.Requests.GetLegalEntityRequest(accountId, accountLegalEntityId));

            return response;
        }

        public async Task DeleteAccountLegalEntity(long accountId, long accountLegalEntityId)
        {
            await _client.Delete(new DeleteAccountLegalEntityRequest(accountId, accountLegalEntityId));
        }

        public async Task ConfirmIncentiveApplication(ConfirmIncentiveApplicationRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _client.PatchWithResponseCode(request);
            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                throw new UlnAlreadySubmittedException();
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestContentException($"Response status code does not indicate success: {(int)response.StatusCode} ({response.StatusCode})", response.StatusCode, response.Body);
            }
        }

        public async Task CreateLegalEntity(long accountId, AccountLegalEntityCreateRequest accountLegalEntity)
        {
            var request = new PutAccountLegalEntityRequest(accountId) {Data = accountLegalEntity};
            await _client.Put(request);
        }

        public async Task SendBankDetailRequiredEmail(long accountId, SendBankDetailsEmailRequest sendBankDetailsEmailRequest)
        {
            var request = new PostBankDetailsRequiredEmailRequest(accountId)
            { Data = sendBankDetailsEmailRequest };

            await _client.Post<SendBankDetailsEmailRequest>(request);
        }

        public async Task SendBankDetailReminderEmail(long accountId, SendBankDetailsEmailRequest sendBankDetailsEmailRequest)
        {
            var request = new PostBankDetailsReminderEmailRequest(accountId)
            { Data = sendBankDetailsEmailRequest };

            await _client.Post<SendBankDetailsEmailRequest>(request);
        }

        public async Task SendBankDetailsRepeatReminderEmails(SendBankDetailsRepeatReminderEmailsRequest sendBankDetailsRepeatReminderEmailsRequest)
        {
            var request = new PostBankDetailsRepeatReminderEmailsRequest { Data = sendBankDetailsRepeatReminderEmailsRequest };

            await _client.Post<SendBankDetailsRepeatReminderEmailsRequest>(request);
        }

        public Task CreateIncentiveApplication(CreateIncentiveApplicationRequestData requestData)
        {
            return _client.Post<CreateIncentiveApplicationRequestData>(new CreateIncentiveApplicationRequest { Data = requestData });
        }

        public Task UpdateIncentiveApplication(UpdateIncentiveApplicationRequestData requestData)
        {
            return _client.Put(new UpdateIncentiveApplicationRequest { Data = requestData });
        }

        public async Task<IncentiveApplicationDto> GetApplication(long accountId, Guid applicationId)
        {
            var response = await _client.Get<IncentiveApplicationDto>(new GetApplicationRequest(accountId, applicationId));

            return response;
        }

        public async Task<long> GetApplicationLegalEntity(long accountId, Guid applicationId)
        {
            var response = await _client.Get<long>(new GetApplicationLegalEntityRequest(accountId, applicationId));

            return response;
        }

        public async Task SignAgreement(long accountId, long accountLegalEntityId, SignAgreementRequest request)
        {
            await _client.Patch(new PatchSignAgreementRequest(accountId, accountLegalEntityId) { Data = request });
        }

        public async Task UpdateVendorRegistrationCaseStatus(UpdateVendorRegistrationCaseStatusRequest request)
        {
            await _client.Patch(new PatchVendorRegistrationCaseStatusRequest(request));
        }

        public async Task<GetIncentiveDetailsResponse> GetIncentiveDetails()
        {
            return await _client.Get<GetIncentiveDetailsResponse>(new GetIncentiveDetailsRequest());
        }

        public async Task<GetApplicationsResponse> GetApprenticeApplications(long accountId, long accountLegalEntityId)
        {
            return await _client.Get<GetApplicationsResponse>(new GetApplicationsRequest(accountId, accountLegalEntityId));
        }

        public Task AddEmployerVendorIdToLegalEntity(string hashedLegalEntityId, string employerVendorId)
        {
            return _client.Put(new PutEmployerVendorIdForLegalEntityRequest(hashedLegalEntityId)
            { Data = new PutEmployerVendorIdForLegalEntityRequestData { EmployerVendorId = employerVendorId } });
        }

        public async Task EarningsResilienceCheck()
        {
            await _client.Post<string>(new EarningsResilenceCheckRequest());
        }

        public async Task UpdateCollectionCalendarPeriod(UpdateCollectionCalendarPeriodRequestData requestData)
        {
            await _client.Patch<UpdateCollectionCalendarPeriodRequestData>(new UpdateCollectionCalendarPeriodRequest { Data = requestData });
        }

        public async Task RefreshLegalEntities(IEnumerable<Accounts.AccountLegalEntity> accountLegalEntities, int pageNumber, int pageSize, int totalPages)
        {
            var accountLegalEntitiesData = new Dictionary<string, object>
            {
                { "AccountLegalEntities", accountLegalEntities },
                { "PageNumber", pageNumber },
                { "PageSize", pageSize },
                { "TotalPages", totalPages }
            };
            var request = new RefreshLegalEntitiesRequestData { Type = JobType.RefreshLegalEntities, Data = accountLegalEntitiesData };
            await _client.Put(new RefreshLegalEntitiesRequest { Data = request });
        }

        public async Task<ApprenticeshipIncentiveDto[]> GetApprenticeshipIncentives(long accountId, long accountLegalEntityId)
        {
            var response = await _client.GetAll<ApprenticeshipIncentiveDto>(new GetApprenticeshipIncentivesRequest(accountId, accountLegalEntityId));

            return response.ToArray();
        }

        public async Task<GetLatestVendorRegistrationCaseUpdateDateTimeResponse> GetLatestVendorRegistrationCaseUpdateDateTime()
        {
            return await _client.Get<GetLatestVendorRegistrationCaseUpdateDateTimeResponse>(new GetLatestVendorRegistrationCaseUpdateDateTimeRequest());
        }

        private async Task VerifyApprenticeshipIsEligible(ApprenticeshipItem apprenticeship, ConcurrentBag<ApprenticeshipItem> bag)
        {
            var statusCode = await _client.GetResponseCode(new GetEligibleApprenticeshipsRequest(apprenticeship.Uln, apprenticeship.StartDate));
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    bag.Add(apprenticeship);
                    break;
                case HttpStatusCode.NotFound:
                    break;
                default:
                    throw new ApplicationException($"Unable to get status for apprentice Uln {apprenticeship.Uln}");
            }
        }


    }
}