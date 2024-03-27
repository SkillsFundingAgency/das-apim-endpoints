using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class RoatpCourseManagementApiHealthCheck : ApiHealthCheck<RoatpV2ApiConfiguration>, IHealthCheck
    {
        public RoatpCourseManagementApiHealthCheck(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> client, ILogger<RoatpCourseManagementApiHealthCheck> logger)
            : base("Roatp Course Management API", client, logger)
        {
        }
    }
}