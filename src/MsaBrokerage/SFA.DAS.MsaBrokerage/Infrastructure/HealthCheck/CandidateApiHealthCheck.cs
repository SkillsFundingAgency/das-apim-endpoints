using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.MsaBrokerage.Infrastructure.HealthCheck;

public class CandidateApiHealthCheck : ApiHealthCheck<CandidateApiConfiguration>, IHealthCheck
{
    public static readonly string HealthCheckDescription = "Candidate Account API";
    public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

    public CandidateApiHealthCheck(
        ICandidateApiClient<CandidateApiConfiguration> client,
        ILogger<CandidateApiHealthCheck> logger)
        : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
    {
    }
}
