using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class ApprenticeFeedbackApiHealthCheck : ApiHealthCheck<ApprenticeFeedbackApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Apprentice Feedback API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

        public ApprenticeFeedbackApiHealthCheck(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> client, ILogger<ApprenticeFeedbackApiHealthCheck> logger)
            : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
        {
        }
    }
}