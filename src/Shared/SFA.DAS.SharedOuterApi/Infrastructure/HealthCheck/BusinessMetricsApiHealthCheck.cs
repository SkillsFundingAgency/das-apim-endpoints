using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class BusinessMetricsApiHealthCheck(
        IBusinessMetricsApiClient<BusinessMetricsConfiguration> client,
        ILogger<BusinessMetricsApiHealthCheck> logger)
        : ApiHealthCheck<BusinessMetricsConfiguration>(HealthCheckDescription, HealthCheckResultDescription, client,
            logger), IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Business Metrics API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";
    }
}