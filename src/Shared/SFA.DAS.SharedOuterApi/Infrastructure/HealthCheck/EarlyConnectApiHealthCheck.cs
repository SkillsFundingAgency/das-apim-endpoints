using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class EarlyConnectApiHealthCheck : ApiHealthCheck<EarlyConnectApiConfiguration>, IHealthCheck
    {
        public EarlyConnectApiHealthCheck(IEarlyConnectApiClient<EarlyConnectApiConfiguration> client, ILogger<EarlyConnectApiHealthCheck> logger)
            : base("EarlyConnect Api", client, logger)
        {
        }
    }
}