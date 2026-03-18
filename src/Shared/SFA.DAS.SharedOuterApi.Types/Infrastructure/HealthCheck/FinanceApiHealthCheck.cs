using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck
{
    public class FinanceApiHealthCheck : ApiHealthCheck<FinanceApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Finance API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";
        public FinanceApiHealthCheck(IFinanceApiClient<FinanceApiConfiguration> client, ILogger<FinanceApiHealthCheck> logger)
            : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
        {
        }
    }
}