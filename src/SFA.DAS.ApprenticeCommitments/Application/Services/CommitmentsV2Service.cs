using SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Services
{
    public class CommitmentsV2Service
    {
        private readonly IInternalApiClient<CommitmentsV2Configuration> _client;

        public CommitmentsV2Service(IInternalApiClient<CommitmentsV2Configuration> client) => _client = client;

        public Task<bool> IsHealthy() => HealthCheck.IsHealthy(_client, new GetCommitmentsPingRequest());

        public async Task<ApprenticeshipResponse> GetApprenticeshipDetails(long accountId, long apprenticeshipId)
        {
            var apprenticeship = await _client.Get<ApprenticeshipResponse>(
                new GetApprenticeshipDetailsRequest(apprenticeshipId));

            if (apprenticeship?.EmployerAccountId != accountId)
            {
                throw new HttpRequestContentException(
                    $"Employer Account {accountId} does not have access to apprenticeship Id {apprenticeshipId}",
                    System.Net.HttpStatusCode.BadRequest,
                    "");
            }

            return apprenticeship;
        }
    }
}