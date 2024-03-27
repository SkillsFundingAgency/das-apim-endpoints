using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class ApprenticeAccountsApiHealthCheck : ApiHealthCheck<ApprenticeAccountsApiConfiguration>, IHealthCheck
    {
        public ApprenticeAccountsApiHealthCheck(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> client, ILogger<ApprenticeAccountsApiHealthCheck> logger)
            : base("Apprentice Accounts Api", client, logger)
        {
        }
    }
}