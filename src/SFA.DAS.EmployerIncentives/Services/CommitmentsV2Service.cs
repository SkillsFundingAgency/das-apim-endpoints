using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Services
{
    public class CommitmentsV2Service : ICommitmentsV2Service
    {
        private readonly IRestApiClient _restApiClient;

        public CommitmentsV2Service(HttpClient client)
        {
            _restApiClient = new RestApiClient(client);
        }

        public async Task<HealthCheckResult> HealthCheck(CancellationToken cancellationToken = default)
        {
            try
            {
                var health = await _restApiClient.GetAsync<CommitmentsV2Health>("/health", cancellationToken);

                if (health == null)
                {
                    return HealthCheckResult.Unhealthy();
                }

                switch (health.Status)
                {
                    case "Healthy" :
                        return HealthCheckResult.Healthy();
                    case "Degraded" :
                        return HealthCheckResult.Degraded();
                    default :
                        return HealthCheckResult.Unhealthy();
                }
            }
            catch
            {
                return HealthCheckResult.Unhealthy();
            }
        }
    }

    public class CommitmentsV2Health
    {
        public string Status { get; set; }
    }
}
