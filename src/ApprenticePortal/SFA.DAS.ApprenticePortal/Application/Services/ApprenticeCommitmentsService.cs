using SFA.DAS.ApprenticePortal.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticePortal.Application.Services
{
    public class ApprenticePortalService
    {
        private readonly IInternalApiClient<ApprenticePortalConfiguration> _client;

        public ApprenticePortalService(IInternalApiClient<ApprenticePortalConfiguration> client)
            => _client = client;

        public Task<bool> IsHealthy() => HealthCheck.IsHealthy(_client);
    }
}