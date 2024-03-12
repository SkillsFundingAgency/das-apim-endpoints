using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.RoatpProviderModeration.Application.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.RoatpProviderModeration.Api.HealthCheck;

[ExcludeFromCodeCoverage]
public class RoatpV2ApiHealthCheck : IHealthCheck
{
    public const string HealthCheckResultDescription = "Roatp V2 API Health Check";

    private readonly IRoatpV2ApiClient _apiClient;

    public RoatpV2ApiHealthCheck(IRoatpV2ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        var response = await _apiClient.GetHealth(cancellationToken);
        return response.IsSuccessStatusCode
            ? HealthCheckResult.Healthy(HealthCheckResultDescription)
            : HealthCheckResult.Unhealthy(HealthCheckResultDescription);
    }
}
