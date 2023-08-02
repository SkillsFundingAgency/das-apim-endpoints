using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Api.HealthCheck;

public class ApprenticeAanInnerApiHealthCheck : IHealthCheck
{
    public const string HealthCheckResultDescription = "Apprentice Aan Inner API Health Check";

    private readonly IAanHubRestApiClient _outerApiClient;
    private readonly ILogger<ApprenticeAanInnerApiHealthCheck> _logger;

    public ApprenticeAanInnerApiHealthCheck(ILogger<ApprenticeAanInnerApiHealthCheck> logger, IAanHubRestApiClient outerApiClient)
    {
        _logger = logger;
        _outerApiClient = outerApiClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        _logger.LogInformation("Apprentice Aan Inner API pinging call");
        try
        {
            await _outerApiClient.GetCalendars(cancellationToken);
            return HealthCheckResult.Healthy(HealthCheckResultDescription);

        }
        catch (Exception)
        {
            _logger.LogError("Apprentice Aan Inner API ping failed");
            return HealthCheckResult.Unhealthy(HealthCheckResultDescription);
        }
    }
}