using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.ProviderPR.Infrastructure;

namespace SFA.DAS.ProviderPR.Api.HealthCheck;

public class ProviderRelationshipsApiHealthCheck(IProviderRelationshipsApiRestClient _apiClient) : IHealthCheck
{
    public const string HealthCheckResultDescription = "Provider Realtionships API Health Check";

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var response = await _apiClient.GetHealth(cancellationToken);
        return response.IsSuccessStatusCode
            ? HealthCheckResult.Healthy(HealthCheckResultDescription)
            : HealthCheckResult.Unhealthy(HealthCheckResultDescription);
    }
}
