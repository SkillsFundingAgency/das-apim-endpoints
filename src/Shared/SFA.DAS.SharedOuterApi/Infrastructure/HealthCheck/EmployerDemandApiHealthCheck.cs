using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class EmployerDemandApiHealthCheck : ApiHealthCheck<EmployerDemandApiConfiguration>, IHealthCheck
    {
        public EmployerDemandApiHealthCheck(IEmployerDemandApiClient<EmployerDemandApiConfiguration> client, ILogger<EmployerDemandApiHealthCheck> logger)
            : base("Employer Demand API", client, logger)
        {
        }
    }
}