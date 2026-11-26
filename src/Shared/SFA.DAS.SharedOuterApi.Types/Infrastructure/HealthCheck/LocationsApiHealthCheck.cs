using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck;

public class LocationsApiHealthCheck(
    ILocationApiClient<LocationApiConfiguration> client,
    ILogger<LocationsApiHealthCheck> logger)
    : ApiHealthCheck<LocationApiConfiguration>(HealthCheckDescription, HealthCheckResultDescription, client, logger),
        IHealthCheck
{
    public static readonly string HealthCheckDescription = "Location API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";
}