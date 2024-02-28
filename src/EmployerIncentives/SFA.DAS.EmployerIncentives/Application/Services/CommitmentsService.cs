using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Commitments;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class CommitmentsService : ICommitmentsService
    {
        private readonly ICommitmentsApiClient<CommitmentsConfiguration> _client;

        public CommitmentsService(ICommitmentsApiClient<CommitmentsConfiguration> client)
        {
            _client = client;
        }

        public async Task<bool> IsHealthy()
        {
            try
            {
                var status = await _client.GetResponseCode(new GetCommitmentsPingRequest());
                return (status == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
        }

        public async Task<ApprenticeshipItem[]> Apprenticeships(long accountId, long accountLegalEntityId, DateTime startDateFrom, DateTime startDateTo)
        {
            var response = await _client.Get<GetApprenticeshipListResponse>(new GetApprenticeshipsRequest(accountId, accountLegalEntityId, startDateFrom, startDateTo));
            return response.Apprenticeships.ToArray();
        }

        public async Task<ApprenticeshipResponse[]> GetApprenticeshipDetails(long accountId, IEnumerable<long> apprenticeshipIds)
        {
            var apprenticeshipResponses = new ConcurrentBag<ApprenticeshipResponse>();

            var tasks = apprenticeshipIds.Select(x => CheckApprenticeshipIsAssignedToAccountAndAddToBag(accountId, x, apprenticeshipResponses));
            await Task.WhenAll(tasks);

            return apprenticeshipResponses.ToArray();
        }

        private async Task CheckApprenticeshipIsAssignedToAccountAndAddToBag(long accountId, long apprenticeshipId, ConcurrentBag<ApprenticeshipResponse> apprenticeshipResponses)
        {
            var apprenticeship = await _client.Get<ApprenticeshipResponse>(new GetApprenticeshipDetailsRequest(apprenticeshipId));
            
            if (apprenticeship.EmployerAccountId != accountId)
            {
                throw new UnauthorizedAccessException($"Employer Account {accountId} does not have access to apprenticeship Id {apprenticeshipId}");
            }

            apprenticeshipResponses.Add(apprenticeship);
        }
    }
}
