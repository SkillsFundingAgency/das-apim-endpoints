using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck;

public class FinanceApiHealthCheck(
    IFinanceApiClient<FinanceApiConfiguration> client,
    ILogger<FinanceApiHealthCheck> logger)
    : ApiHealthCheck<FinanceApiConfiguration>(HealthCheckDescription, HealthCheckResultDescription, client, logger),
        IHealthCheck
{
    public static readonly string HealthCheckDescription = "Finance API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";
}