using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class TrainingProviderApiHealthCheck : ApiHealthCheck<TrainingProviderConfiguration>, IHealthCheck
    {
        public TrainingProviderApiHealthCheck(ITrainingProviderApiClient<TrainingProviderConfiguration> client, ILogger<TrainingProviderApiHealthCheck> logger)
            : base("Training Provider Api", client, logger)
        {
        }
    }
}