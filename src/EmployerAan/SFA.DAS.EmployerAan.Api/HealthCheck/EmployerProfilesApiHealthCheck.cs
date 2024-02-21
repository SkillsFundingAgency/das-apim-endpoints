using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Api.HealthCheck;

public class EmployerProfilesApiHealthCheck : IHealthCheck
{
    public const string HealthCheckResultDescription = "Employer Profiles API Health Check";

    private readonly IEmployerProfilesApiClient _apiClient;

    public EmployerProfilesApiHealthCheck(IEmployerProfilesApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        var response = await _apiClient.GetHealth(cancellationToken);
        return response.IsSuccessStatusCode
            ? HealthCheckResult.Healthy(HealthCheckResultDescription)
            : HealthCheckResult.Unhealthy(HealthCheckResultDescription);
    }
}