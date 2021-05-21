using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.EarningsResilienceCheck;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerIncentives.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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