using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck;

public class CollectionCalendarApiHealthCheck(
    ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> client,
    ILogger<CollectionCalendarApiHealthCheck> logger)
    : ApiHealthCheck<CollectionCalendarApiConfiguration>(HealthCheckDescription, HealthCheckResultDescription, client,
        logger), IHealthCheck
{
    public static readonly string HealthCheckDescription = "Collection Calendar API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";
}