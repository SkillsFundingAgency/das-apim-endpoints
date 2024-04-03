using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
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