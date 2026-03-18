using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck
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
