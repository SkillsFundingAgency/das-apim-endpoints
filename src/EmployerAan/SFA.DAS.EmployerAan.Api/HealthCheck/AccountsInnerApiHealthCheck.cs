using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Api.HealthCheck;

public class AccountsInnerApiHealthCheck : IHealthCheck
{
    public const string HealthCheckResultDescription = "Accounts Inner API Health Check";

    private readonly IAccountsApiClient _apiClient;

    public AccountsInnerApiHealthCheck(IAccountsApiClient apiClient)
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