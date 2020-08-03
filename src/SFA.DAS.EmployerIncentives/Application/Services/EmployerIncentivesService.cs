using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
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

        public async Task<bool> IsHealthy(CancellationToken cancellationToken = default)
        {
            try
            {
                var status = await _client.GetResponseCode(new GetPingRequest());
                return (status == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
        }

        public async Task<ApprenticeshipItem[]> GetEligibleApprenticeships(IEnumerable<ApprenticeshipItem> allApprenticeship, CancellationToken cancellationToken = default)
        {
            var bag = new ConcurrentBag<ApprenticeshipItem>();
            var tasks = allApprenticeship.Select(x => VerifyApprenticeshipIsEligible(x, bag, cancellationToken));
            await Task.WhenAll(tasks);

            return bag.ToArray();
        }

        public async Task<GetAccountLegalEntitiesResponse> GetAccountLegalEntities(long accountId)
        {
            var response = await _client.Get<GetAccountLegalEntitiesResponse>(new GetAccountLegalEntitiesRequest(accountId));

            return response;
        }

        public async Task DeleteAccountLegalEntity(long accountId, long accountLegalEntityId)
        {
            await _client.Delete(new DeleteAccountLegalEntityRequest(accountId, accountLegalEntityId));
        }

        public async Task ConfirmIncentiveApplication(ConfirmIncentiveApplicationRequest request, CancellationToken cancellationToken = default)
        {
            await _client.Patch<void>(request);
        }

        public async Task<AccountLegalEntity> CreateLegalEntity(long accountId, AccountLegalEntityCreateRequest accountLegalEntity)
        {
            var result = await _client.Post<AccountLegalEntity>(new PostAccountLegalEntityRequest(accountId)
            { Data = accountLegalEntity });

            return result;
        }

        private async Task VerifyApprenticeshipIsEligible(ApprenticeshipItem apprenticeship, ConcurrentBag<ApprenticeshipItem> bag, CancellationToken cancellationToken)
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