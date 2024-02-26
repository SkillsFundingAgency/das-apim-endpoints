using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Api.HealthCheck;

public class ReferenceDataApiHealthCheck : IHealthCheck
{
    public const string HealthCheckResultDescription = "Reference Data API Health Check";

    private readonly IReferenceDataApiClient _referenceDataApiClient;

    public ReferenceDataApiHealthCheck(IReferenceDataApiClient referenceDataApiClient)
    {
        _referenceDataApiClient = referenceDataApiClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        var response = await _referenceDataApiClient.GetHealth(cancellationToken);
        return response.IsSuccessStatusCode
            ? HealthCheckResult.Healthy(HealthCheckResultDescription)
            : HealthCheckResult.Unhealthy(HealthCheckResultDescription);
    }
}