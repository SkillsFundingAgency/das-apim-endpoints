using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;

public class ProviderRelationshipsApiHealthCheck : ApiHealthCheck<RoatpV2ApiConfiguration>, IHealthCheck
{
    public static readonly string HealthCheckDescription = "Provider Relationships API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

    public ProviderRelationshipsApiHealthCheck(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> client, ILogger<ProviderRelationshipsApiHealthCheck> logger)
        : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
    {
    }
}
