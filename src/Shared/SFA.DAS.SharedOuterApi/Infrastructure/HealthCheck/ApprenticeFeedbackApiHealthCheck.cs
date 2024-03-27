using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class ApprenticeFeedbackApiHealthCheck : ApiHealthCheck<ApprenticeFeedbackApiConfiguration>, IHealthCheck
    {
        public ApprenticeFeedbackApiHealthCheck(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> client, ILogger<ApprenticeFeedbackApiHealthCheck> logger)
            : base("Apprentice Feedback API", client, logger)
        {
        }
    }
}