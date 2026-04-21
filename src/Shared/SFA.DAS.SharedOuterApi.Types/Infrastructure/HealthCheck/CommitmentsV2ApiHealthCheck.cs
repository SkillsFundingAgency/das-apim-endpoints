using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck;

public class CommitmentsV2ApiHealthCheck(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> client,
    ILogger<CommitmentsV2ApiHealthCheck> logger)
    : ApiHealthCheck<CommitmentsV2ApiConfiguration>(HealthCheckDescription, HealthCheckResultDescription, client,
        logger), IHealthCheck
{
    public static readonly string HealthCheckDescription = "Employer Commitments V2 API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";
}