using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class CommitmentsV2HealthCheck : ApiHealthCheck<CommitmentsV2ApiConfiguration>, IHealthCheck
    {
        public CommitmentsV2HealthCheck(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> client, ILogger<CommitmentsV2HealthCheck> logger)
            : base("Employer Commitments V2 Api", client, logger)
        {
        }
    }
}