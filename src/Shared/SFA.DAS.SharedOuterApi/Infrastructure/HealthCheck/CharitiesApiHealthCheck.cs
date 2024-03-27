using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class CharitiesApiHealthCheck : ApiHealthCheck<CharitiesApiConfiguration>, IHealthCheck
    {
        public CharitiesApiHealthCheck(ICharitiesApiClient<CharitiesApiConfiguration> client, ILogger<CharitiesApiHealthCheck> logger)
            : base("Charities Api", client, logger)
        {
        }
    }
}
