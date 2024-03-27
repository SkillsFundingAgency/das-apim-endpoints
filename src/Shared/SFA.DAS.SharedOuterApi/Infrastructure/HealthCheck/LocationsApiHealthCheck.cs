using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class LocationsApiHealthCheck : ApiHealthCheck<LocationApiConfiguration>, IHealthCheck
    {
        public LocationsApiHealthCheck(ILocationApiClient<LocationApiConfiguration> client, ILogger<LocationsApiHealthCheck> logger)
            : base("Location API", client, logger)
        {
        }
    }
}