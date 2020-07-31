using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models.Commitments;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.Services
{
    public class CommitmentsV2Service : ICommitmentsV2Service
    {
        private readonly IRestApiClient _restApiClient;

        public CommitmentsV2Service(HttpClient client)
        {
            _restApiClient = new RestApiClient(client);
        }

        public async Task<bool> IsHealthy(CancellationToken cancellationToken = default)
        {
            try
            {
                var status = await _restApiClient.GetHttpStatusCode("api/ping", cancellationToken);
                    
                return (status == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<ApprenticeshipItem>> Apprenticeships(long accountId, long accountLegalEntityId,
            CancellationToken cancellationToken = default)
        {
            var response = await _restApiClient.Get<ApprenticeshipSearchResponse>("api/apprenticeships",
                new { accountId, accountLegalEntityId }, cancellationToken);

            return response.Apprenticeships;
        }

        public async Task<ApprenticeshipResponse[]> GetApprenticeshipDetails(long accountId, IEnumerable<long> apprenticeshipIds, CancellationToken cancellationToken = default)
        {
            ConcurrentBag<ApprenticeshipResponse> bag = new ConcurrentBag<ApprenticeshipResponse>();

            var tasks = apprenticeshipIds.Select(x => CheckApprenticeshipIsAssignedToAccountAndAddToBag(accountId, x, bag, cancellationToken));
            await Task.WhenAll(tasks);

            return bag.ToArray();
        }

        private async Task CheckApprenticeshipIsAssignedToAccountAndAddToBag(long accountId, long apprenticeshipId, ConcurrentBag<ApprenticeshipResponse> bag, CancellationToken cancellationToken)
        {
            var apprenticeship = await _restApiClient.Get<ApprenticeshipResponse>($"api/apprenticeship/{apprenticeshipId}", null, cancellationToken);

            if (apprenticeship.EmployerAccountId != accountId)
            {
                throw new UnauthorizedAccessException($"Employer Account {accountId} does not have access to apprenticeship Id {apprenticeshipId}");
            } 

            bag.Add(apprenticeship);
        }
    }
}
