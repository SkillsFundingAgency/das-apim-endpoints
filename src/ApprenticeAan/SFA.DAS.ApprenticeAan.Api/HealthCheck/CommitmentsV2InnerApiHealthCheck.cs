using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Api.HealthCheck;

public class CommitmentsV2InnerApiHealthCheck : IHealthCheck
{
    public const string HealthCheckResultDescription = "Commitments V2 API Health Check";

    private readonly ICommitmentsV2InnerApiClient _commitmentsV2InnerApiClient;

    public CommitmentsV2InnerApiHealthCheck(ICommitmentsV2InnerApiClient commitmentsV2InnerApiClient)
    {
        _commitmentsV2InnerApiClient = commitmentsV2InnerApiClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        var response = await _commitmentsV2InnerApiClient.GetHealth(cancellationToken);
        return response.IsSuccessStatusCode
            ? HealthCheckResult.Healthy(HealthCheckResultDescription)
            : HealthCheckResult.Unhealthy(HealthCheckResultDescription);
    }
}