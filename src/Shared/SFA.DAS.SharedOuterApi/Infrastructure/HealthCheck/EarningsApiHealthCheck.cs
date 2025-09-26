using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
public class EarningsApiHealthCheck : ApiHealthCheck<EarningsApiConfiguration>, IHealthCheck
{
    public static readonly string HealthCheckDescription = "Earnings API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

    public EarningsApiHealthCheck(IEarningsApiClient<EarningsApiConfiguration> client, ILogger<EarningsApiHealthCheck> logger)
        : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
    {
    }
}