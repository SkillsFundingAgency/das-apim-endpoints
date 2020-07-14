using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Services
{
    public class EmployerIncentivesService : IEmployerIncentivesService
    {
        private readonly IRestApiClient _client;

        public EmployerIncentivesService(HttpClient httpClient)
        {
            _client = new RestApiClient(httpClient);
        }

        public async Task<HealthCheckResult> HealthCheck(CancellationToken cancellationToken = default)
        {
            try
            {
                var value = await _client.Get("/health", null, cancellationToken);
                switch (value)
                {
                    case "Healthy":
                        return HealthCheckResult.Healthy();
                    case "Degraded":
                        return HealthCheckResult.Degraded();
                    default:
                        return HealthCheckResult.Unhealthy();
                }
            }
            catch
            {
                return HealthCheckResult.Unhealthy();
            }
        }
    }
}
