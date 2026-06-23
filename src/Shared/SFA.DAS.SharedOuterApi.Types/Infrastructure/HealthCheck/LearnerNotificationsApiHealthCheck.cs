using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck
{
    public class LearnerNotificationsApiHealthCheck : ApiHealthCheck<LearnerNotificationsApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Learner Notifications API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

        public LearnerNotificationsApiHealthCheck(
            ILearnerNotificationsInnerApiClient<LearnerNotificationsApiConfiguration> client, 
            ILogger<LearnerNotificationsApiHealthCheck> logger)
            : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
        {
        }
    }
}