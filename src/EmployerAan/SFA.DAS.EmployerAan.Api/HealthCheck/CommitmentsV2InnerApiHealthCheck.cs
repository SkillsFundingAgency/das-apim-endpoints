using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Api.HealthCheck;

public class CommitmentsV2InnerApiHealthCheck : IHealthCheck
{
    public const string HealthCheckResultDescription = "Commitments V2 API Health Check";

    private readonly ICommitmentsV2ApiClient _commitmentsV2ApiClient;

    public CommitmentsV2InnerApiHealthCheck(ICommitmentsV2ApiClient commitmentsV2InnerApiClient)
    {
        _commitmentsV2ApiClient = commitmentsV2InnerApiClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        var response = await _commitmentsV2ApiClient.GetHealth(cancellationToken);
        return response.IsSuccessStatusCode
            ? HealthCheckResult.Healthy(HealthCheckResultDescription)
            : HealthCheckResult.Unhealthy(HealthCheckResultDescription);
    }
}