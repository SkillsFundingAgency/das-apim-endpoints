using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.LearnerData.InnerApi;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Api.HealthCheck;

public class LearnerDataApiHealthCheck : ApiHealthCheck<LearnerDataInnerApiConfiguration>, IHealthCheck
{
    public static readonly string HealthCheckDescription = "Learner Data API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

    public LearnerDataApiHealthCheck(IInternalApiClient<LearnerDataInnerApiConfiguration> client, ILogger<LearnerDataApiHealthCheck> logger)
        : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
    {
    }
}
