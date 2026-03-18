using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck
{
    public class RoatpCourseManagementApiHealthCheck : ApiHealthCheck<RoatpV2ApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Roatp Course Management API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

        public RoatpCourseManagementApiHealthCheck(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> client, ILogger<RoatpCourseManagementApiHealthCheck> logger)
            : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
        {
        }
    }
}