using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class AssessorsApiHealthCheck : ApiHealthCheck<AssessorsApiConfiguration>, IHealthCheck
    {
        public AssessorsApiHealthCheck(IAssessorsApiClient<AssessorsApiConfiguration> client, ILogger<AssessorsApiHealthCheck> logger)
            : base("Assessors Api", client, logger)
        {
        }
    }
}