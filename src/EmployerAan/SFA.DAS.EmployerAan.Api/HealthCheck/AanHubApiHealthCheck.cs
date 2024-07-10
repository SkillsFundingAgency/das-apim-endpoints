using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Api.HealthCheck;

public class AanHubApiHealthCheck : IHealthCheck
{
    public const string HealthCheckResultDescription = "Aan Hub API Health Check";

    private readonly IAanHubRestApiClient _aanHubApiClient;

    public AanHubApiHealthCheck(IAanHubRestApiClient aanHubApiClient)
    {
        _aanHubApiClient = aanHubApiClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        var response = await _aanHubApiClient.GetHealth(cancellationToken);
        return response.IsSuccessStatusCode
            ? HealthCheckResult.Healthy(HealthCheckResultDescription)
            : HealthCheckResult.Unhealthy(HealthCheckResultDescription);
    }
}