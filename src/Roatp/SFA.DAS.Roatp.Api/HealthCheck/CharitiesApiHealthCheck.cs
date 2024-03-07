using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Roatp.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.Api.HealthCheck;

public class CharitiesApiHealthCheck : IHealthCheck
{
    public const string HealthCheckResultDescription = "Charities Commission API Health Check";

    private readonly ICharitiesRestApiClient _charitiesApiClient;

    public CharitiesApiHealthCheck(ICharitiesRestApiClient charitiesApiClient)
    {
        _charitiesApiClient = charitiesApiClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        var response = await _charitiesApiClient.GetHealth(cancellationToken);
        return response.IsSuccessStatusCode
            ? HealthCheckResult.Healthy(HealthCheckResultDescription)
            : HealthCheckResult.Unhealthy(HealthCheckResultDescription);
    }
}
