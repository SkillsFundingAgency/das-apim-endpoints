using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;

public class PensionsRegulatorApiHealthCheck : ApiHealthCheck<PensionRegulatorApiConfiguration>, IHealthCheck
{
    public static readonly string HealthCheckDescription = "Pensions Regulator API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

    public PensionsRegulatorApiHealthCheck(IProviderCoursesApiClient<PensionRegulatorApiConfiguration> client, ILogger<PensionsRegulatorApiHealthCheck> logger)
        : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
    {
    }
}