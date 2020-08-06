using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
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

        public Task CreateIncentiveApplication(CreateIncentiveApplicationRequest request)
        {
            return _client.Post<CreateIncentiveApplicationRequest>(new PostCreateIncentiveApplicationRequest { Data = request });
        }

        public Task UpdateIncentiveApplication(UpdateIncentiveApplicationRequest request)
        {
            return _client.Put(new PutIncentiveApplicationRequest { Data = request });
        }

        public async Task<IncentiveApplicationDto> GetApplication(long accountId, Guid applicationId)
        {
            var response = await _client.Get<IncentiveApplicationDto>(new GetApplicationRequest(accountId, applicationId));

            return response;
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