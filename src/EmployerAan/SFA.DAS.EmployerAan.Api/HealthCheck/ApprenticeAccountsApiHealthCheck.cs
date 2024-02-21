using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Api.HealthCheck;

public class ApprenticeAccountsApiHealthCheck : IHealthCheck
{
    public const string HealthCheckResultDescription = "Apprentice Accounts API Health Check";

    private readonly IApprenticeAccountsApiClient _apprenticeAccountsApiClient;

    public ApprenticeAccountsApiHealthCheck(IApprenticeAccountsApiClient apprenticeAccountsApiClient)
    {
        _apprenticeAccountsApiClient = apprenticeAccountsApiClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        var response = await _apprenticeAccountsApiClient.GetHealth(cancellationToken);
        return response.IsSuccessStatusCode
            ? HealthCheckResult.Healthy(HealthCheckResultDescription)
            : HealthCheckResult.Unhealthy(HealthCheckResultDescription);
    }
}