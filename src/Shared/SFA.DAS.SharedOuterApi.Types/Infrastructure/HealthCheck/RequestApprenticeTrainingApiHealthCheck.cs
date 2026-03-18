using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck
{
    public class RequestApprenticeTrainingApiHealthCheck : ApiHealthCheck<RequestApprenticeTrainingApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Request Apprentice Training API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";
        public RequestApprenticeTrainingApiHealthCheck(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> client, ILogger<RequestApprenticeTrainingApiHealthCheck> logger)
            : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
        {
        }
    }
}