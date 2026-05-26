using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck;

public class PensionsRegulatorApiHealthCheck(
    IProviderCoursesApiClient<PensionRegulatorApiConfiguration> client,
    ILogger<PensionsRegulatorApiHealthCheck> logger)
    : ApiHealthCheck<PensionRegulatorApiConfiguration>(HealthCheckDescription, HealthCheckResultDescription, client,
        logger), IHealthCheck
{
    public static readonly string HealthCheckDescription = "Pensions Regulator API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";
}