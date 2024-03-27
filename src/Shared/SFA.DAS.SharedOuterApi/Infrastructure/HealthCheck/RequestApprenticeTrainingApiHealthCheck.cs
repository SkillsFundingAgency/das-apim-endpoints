using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class RequestApprenticeTrainingApiHealthCheck : ApiHealthCheck<RequestApprenticeTrainingApiConfiguration>, IHealthCheck
    {
        public RequestApprenticeTrainingApiHealthCheck(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> client, ILogger<RequestApprenticeTrainingApiHealthCheck> logger)
            : base("Request Apprentice Training API", client, logger)
        {
        }
    }
}