using SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
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
            var apprenticeship = await GetApprenticeshipDetails(apprenticeshipId);

            if (apprenticeship?.EmployerAccountId != accountId)
            {
                throw new HttpRequestContentException(
                    $"Employer Account {accountId} does not have access to apprenticeship Id {apprenticeshipId}",
                    System.Net.HttpStatusCode.BadRequest,
                    "");
            }

            return apprenticeship;
        }

        public async Task<ApprenticeshipResponse> GetApprenticeshipDetails(long apprenticeshipId)
        {
            var response = await _client.GetWithResponseCode<ApprenticeshipResponse>(new GetApprenticeshipDetailsRequest(apprenticeshipId));

            if (response.StatusCode != HttpStatusCode.OK)
                throw new HttpRequestContentException(response.ErrorContent, response.StatusCode);

            return response.Body;
        }
    }
}