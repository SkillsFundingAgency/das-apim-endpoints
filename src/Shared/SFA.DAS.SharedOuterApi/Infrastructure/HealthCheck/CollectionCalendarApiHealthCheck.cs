using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
public class CollectionCalendarApiHealthCheck : ApiHealthCheck<CollectionCalendarApiConfiguration>, IHealthCheck
{
    public static readonly string HealthCheckDescription = "Collection Calendar API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

    public CollectionCalendarApiHealthCheck(ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> client, ILogger<CollectionCalendarApiHealthCheck> logger)
        : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
    {
    }
}