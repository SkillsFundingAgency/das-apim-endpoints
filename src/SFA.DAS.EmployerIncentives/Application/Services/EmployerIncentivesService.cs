using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
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
            var response = await _client.Get<AccountLegalEntity>(new GetLegalEntityRequest(accountId, accountLegalEntityId));

            return response;
        }

        public async Task DeleteAccountLegalEntity(long accountId, long accountLegalEntityId)
        {
            await _client.Delete(new DeleteAccountLegalEntityRequest(accountId, accountLegalEntityId));
        }

        public async Task ConfirmIncentiveApplication(ConfirmIncentiveApplicationRequest request, CancellationToken cancellationToken = default)
        {
            await _client.Patch(request);
        }

        public async Task<AccountLegalEntity> CreateLegalEntity(long accountId, AccountLegalEntityCreateRequest accountLegalEntity)
        {
            var result = await _client.Post<AccountLegalEntity>(new PostAccountLegalEntityRequest(accountId)
            { Data = accountLegalEntity });

            return result;
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
            await _client.Patch(new PatchSignAgreementRequest(accountId, accountLegalEntityId) {Data = request});
        }

        public async Task<GetIncentiveDetailsResponse> GetIncentiveDetails()
        {
            return await _client.Get<GetIncentiveDetailsResponse>(new GetIncentiveDetailsRequest());
        }

        public async Task<IEnumerable<ApprenticeApplication>> GetApprenticeApplications(long accountId)
        {
            return await _client.Get<IEnumerable<ApprenticeApplication>>(new GetApplicationsRequest(accountId));
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