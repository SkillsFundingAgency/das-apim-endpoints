using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Roatp.Infrastructure;

namespace SFA.DAS.Roatp.Api.HealthCheck;

public class RoatpApiHealthCheck(IRoatpApiClient _roatpApiClient) : IHealthCheck
{
    public const string HealthCheckResultDescription = "RoATP API Health Check";

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        var response = await _roatpApiClient.GetHealth(cancellationToken);
        return response.IsSuccessStatusCode
            ? HealthCheckResult.Healthy(HealthCheckResultDescription)
            : HealthCheckResult.Unhealthy(HealthCheckResultDescription);
    }
}
