using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class EmployerDemandApiHealthCheck : ApiHealthCheck<EmployerDemandApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Employer Demand API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

        public EmployerDemandApiHealthCheck(IEmployerDemandApiClient<EmployerDemandApiConfiguration> client, ILogger<EmployerDemandApiHealthCheck> logger)
            : base(HealthCheckDescription, client, logger)
        {
        }
    }
}