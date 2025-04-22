using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.LearnerData.InnerApi;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Api.HealthCheck;

public class LearnerDataApiHealthCheck(
    IInternalApiClient<LearnerDataInnerApiConfiguration> client,
    ILogger<LearnerDataApiHealthCheck> logger)
    : ApiHealthCheck<LearnerDataInnerApiConfiguration>(HealthCheckDescription, HealthCheckResultDescription, client, logger), IHealthCheck
{
    private const string HealthCheckDescription = "Learner Data API";

    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";
}