using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class LocationsApiHealthCheck : ApiHealthCheck<LocationApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Location API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";
        public LocationsApiHealthCheck(ILocationApiClient<LocationApiConfiguration> client, ILogger<LocationsApiHealthCheck> logger)
            : base(HealthCheckDescription, client, logger)
        {
        }
    }
}