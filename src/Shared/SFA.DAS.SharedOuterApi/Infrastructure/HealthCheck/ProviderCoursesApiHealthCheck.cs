using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class ProviderCoursesApiHealthCheck : ApiHealthCheck<ProviderCoursesApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Provider Courses API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

        public ProviderCoursesApiHealthCheck(IProviderCoursesApiClient<ProviderCoursesApiConfiguration> client, ILogger<ProviderCoursesApiHealthCheck> logger)
            : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
        {
        }
    }
}