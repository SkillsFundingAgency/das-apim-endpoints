using SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.ApprenticeCommitments.Application.Services
{
    public class CommitmentsV2Service
    {
        private readonly IInternalApiClient<CommitmentsV2Configuration> _client;
        private readonly ILogger<CommitmentsV2Service> _logger;

        public CommitmentsV2Service(IInternalApiClient<CommitmentsV2Configuration> client, ILogger<CommitmentsV2Service> logger)
        {
            _client = client;
            _logger = logger;
        }

        public Task<bool> IsHealthy() => HealthCheck.IsHealthy(_client, new GetCommitmentsPingRequest());

        public async Task<ApprenticeshipResponse> GetApprenticeshipDetails(long accountId, long apprenticeshipId)
        {
            _logger.LogInformation("Getting Apprenticeship {apprenticeshipId}", apprenticeshipId);
            var apprenticeship = await GetApprenticeshipDetails(apprenticeshipId);

            _logger.LogInformation("Checking Apprenticeship is assigned to Employer Account {accountId}", accountId);
            if (apprenticeship?.EmployerAccountId != accountId)
            {
                throw new HttpRequestContentException(
                    $"Employer Account {accountId} does not have access to apprenticeship Id {apprenticeshipId}",
                    System.Net.HttpStatusCode.BadRequest,
                    "");
            }

            return apprenticeship;
        }

        public Task<ApprenticeshipResponse> GetApprenticeshipDetails(long apprenticeshipId)
        {
            return _client.Get<ApprenticeshipResponse>(new GetApprenticeshipDetailsRequest(apprenticeshipId));
        }
    }
}