using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck;

public class RoatpCourseManagementApiHealthCheck(
    IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> client,
    ILogger<RoatpCourseManagementApiHealthCheck> logger)
    : ApiHealthCheck<RoatpV2ApiConfiguration>(HealthCheckDescription, HealthCheckResultDescription, client, logger),
        IHealthCheck
{
    public static readonly string HealthCheckDescription = "Roatp Course Management API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";
}