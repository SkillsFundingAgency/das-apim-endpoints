using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Api.HealthCheck;

public class ApprenticeAccountsApiHealthCheck : IHealthCheck
{
    public const string HealthCheckResultDescription = "Apprentice Accounts API Health Check";

    private readonly IApprenticeAccountsApiClient _apprenticeAccountsApiClient;

    public ApprenticeAccountsApiHealthCheck(IApprenticeAccountsApiClient apprenticeAccountsApiClient)
    {
        _apprenticeAccountsApiClient = apprenticeAccountsApiClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        var response = await _apprenticeAccountsApiClient.GetHealth(cancellationToken);
        return response.IsSuccessStatusCode
            ? HealthCheckResult.Healthy(HealthCheckResultDescription)
            : HealthCheckResult.Unhealthy(HealthCheckResultDescription);
    }
}