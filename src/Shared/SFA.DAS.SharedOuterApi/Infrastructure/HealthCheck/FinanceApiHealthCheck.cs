using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class FinanceApiHealthCheck : ApiHealthCheck<FinanceApiConfiguration>, IHealthCheck
    {
        public FinanceApiHealthCheck(IFinanceApiClient<FinanceApiConfiguration> client, ILogger<FinanceApiHealthCheck> logger)
            : base ("Finance API", client, logger)
        {
        }
    }
}