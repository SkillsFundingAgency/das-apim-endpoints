using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Services
{
    public class ApprenticeCommitmentsService
    {
        private readonly IInternalApiClient<ApprenticeCommitmentsConfiguration> _client;

        public ApprenticeCommitmentsService(IInternalApiClient<ApprenticeCommitmentsConfiguration> client)
            => _client = client;

        public Task<bool> IsHealthy() => HealthCheck.IsHealthy(_client);

        public Task CreateApprenticeship(ApprovalCreatedRequestData data)
        {
            return _client.Post(new ApprovalCreatedRequest
            {
                Data = data
            });
        }

        public Task ChangeRegistration(ChangeRegistrationRequestData data)
            => _client.Put(new ChangeApprenticeshipRequest { Data = data });
    }
}