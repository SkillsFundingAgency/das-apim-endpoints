using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck
{
    public class EmployerDemandApiHealthCheck : ApiHealthCheck<EmployerDemandApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Employer Demand API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

        public EmployerDemandApiHealthCheck(IEmployerDemandApiClient<EmployerDemandApiConfiguration> client, ILogger<EmployerDemandApiHealthCheck> logger)
            : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
        {
        }
    }
}