using SFA.DAS.ApprenticePortal.Apis.CommitmentsV2InnerApi;
using SFA.DAS.ApprenticePortal.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticePortal.Application.Services
{
    public class CommitmentsV2Service
    {
        private readonly IInternalApiClient<CommitmentsV2Configuration> _client;

        public CommitmentsV2Service(IInternalApiClient<CommitmentsV2Configuration> client) => _client = client;

        public Task<bool> IsHealthy() => HealthCheck.IsHealthy(_client, new GetCommitmentsPingRequest());
    }
}