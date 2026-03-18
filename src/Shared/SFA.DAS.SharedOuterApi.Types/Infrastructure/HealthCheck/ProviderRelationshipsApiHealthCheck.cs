using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck;

public class ProviderRelationshipsApiHealthCheck : ApiHealthCheck<RoatpV2ApiConfiguration>, IHealthCheck
{
    public static readonly string HealthCheckDescription = "Provider Relationships API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

    public ProviderRelationshipsApiHealthCheck(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> client, ILogger<ProviderRelationshipsApiHealthCheck> logger)
        : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
    {
    }
}
