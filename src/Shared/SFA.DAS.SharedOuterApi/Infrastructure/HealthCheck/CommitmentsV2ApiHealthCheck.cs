using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class CommitmentsV2ApiHealthCheck : ApiHealthCheck<CommitmentsV2ApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Employer Commitments V2 API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

        public CommitmentsV2ApiHealthCheck(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> client, ILogger<CommitmentsV2ApiHealthCheck> logger)
            : base(HealthCheckDescription, client, logger)
        {
        }
    }
}