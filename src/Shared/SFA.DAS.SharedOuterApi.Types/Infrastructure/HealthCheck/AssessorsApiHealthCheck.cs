using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck
{
    public class AssessorsApiHealthCheck : ApiHealthCheck<AssessorsApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Assessors API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

        public AssessorsApiHealthCheck(IAssessorsApiClient<AssessorsApiConfiguration> client, ILogger<AssessorsApiHealthCheck> logger)
            : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
        {
        }
    }
}