using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck;
public class LearningApiHealthCheck : ApiHealthCheck<LearningApiConfiguration>, IHealthCheck
{
    public static readonly string HealthCheckDescription = "Learning API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

    public LearningApiHealthCheck(ILearningApiClient<LearningApiConfiguration> client, ILogger<LearningApiHealthCheck> logger)
        : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
    {
    }
}