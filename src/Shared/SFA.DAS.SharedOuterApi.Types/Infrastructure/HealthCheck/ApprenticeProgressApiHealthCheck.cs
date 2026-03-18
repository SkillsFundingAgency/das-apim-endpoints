using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck
{
    public class ApprenticeProgressApiHealthCheck : ApiHealthCheck<ApprenticeProgressApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Apprentice Progress API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

        public ApprenticeProgressApiHealthCheck(IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> client, ILogger<ApprenticeProgressApiHealthCheck> logger)
            : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
        {
        }
    }
}