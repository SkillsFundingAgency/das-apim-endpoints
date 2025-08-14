using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
public class LearningApiHealthCheck : ApiHealthCheck<LearningApiConfiguration>, IHealthCheck
{
    public static readonly string HealthCheckDescription = "Learning API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

    public LearningApiHealthCheck(ILearningApiClient<LearningApiConfiguration> client, ILogger<LearningApiHealthCheck> logger)
        : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
    {
    }
}