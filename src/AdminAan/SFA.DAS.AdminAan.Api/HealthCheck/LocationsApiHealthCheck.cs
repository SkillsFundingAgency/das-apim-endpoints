using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Api.HealthCheck;

public class LocationsApiHealthCheck : IHealthCheck
{
    public const string HealthCheckResultDescription = "Locations API Health Check";

    private readonly ILocationApiClient _locationApiClient;

    public LocationsApiHealthCheck(ILocationApiClient locationApiClient)
    {
        _locationApiClient = locationApiClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        var response = await _locationApiClient.GetHealth(cancellationToken);
        return response.IsSuccessStatusCode
            ? HealthCheckResult.Healthy(HealthCheckResultDescription)
            : HealthCheckResult.Unhealthy(HealthCheckResultDescription);
    }
}