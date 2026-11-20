using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SFA.DAS.PAS.Infrastructure;

public class PasApiHealthCheck(IPasApiClient _pasApiClient) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        await _pasApiClient.HealthCheck(cancellationToken);
        return HealthCheckResult.Healthy();
    }
}
