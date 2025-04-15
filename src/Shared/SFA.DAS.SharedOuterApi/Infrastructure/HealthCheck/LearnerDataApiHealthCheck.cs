using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
public class LearnerDataApiHealthCheck : ApiHealthCheck<LearnerDataApiConfiguration>, IHealthCheck
{
    public static readonly string HealthCheckDescription = "Learner Data API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

    public LearnerDataApiHealthCheck(ILearnerDataApiClient<LearnerDataApiConfiguration> client, ILogger<LearnerDataApiHealthCheck> logger)
        : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
    {
    }
}