using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class CharitiesApiHealthCheck : ApiHealthCheck<CharitiesApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Charities API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

        public CharitiesApiHealthCheck(ICharitiesApiClient<CharitiesApiConfiguration> client, ILogger<CharitiesApiHealthCheck> logger)
            : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
        {
        }
    }
}
