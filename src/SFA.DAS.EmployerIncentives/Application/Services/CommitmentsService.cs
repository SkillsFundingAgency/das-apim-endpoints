using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Commitments;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Services
{
    public class CommitmentsService : ICommitmentsService
    {
        private readonly ICommitmentsApiClient<CommitmentsConfiguration> _client;
        private readonly ILogger<CommitmentsService> _logger;

        public CommitmentsService(ICommitmentsApiClient<CommitmentsConfiguration> client, ILogger<CommitmentsService> logger)
        {
            _client = client;
            _logger = logger;
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
            var json = JsonConvert.SerializeObject(apprenticeship);
            _logger.LogInformation($"Response from Commitments API: {json}");
            if (apprenticeship.EmployerAccountId != accountId)
            {
                throw new UnauthorizedAccessException($"Employer Account {accountId} does not have access to apprenticeship Id {apprenticeshipId}");
            }

            apprenticeshipResponses.Add(apprenticeship);
        }
    }
}
