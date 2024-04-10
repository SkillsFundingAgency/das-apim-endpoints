using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class CoursesApiHealthCheck : ApiHealthCheck<CoursesApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Courses API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";
        public CoursesApiHealthCheck(ICoursesApiClient<CoursesApiConfiguration> client, ILogger<CoursesApiHealthCheck> logger)
            : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
        {
        }
    }
}