using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class AccountsApiHealthCheck : ApiHealthCheck<AccountsConfiguration>, IHealthCheck
    {
        public AccountsApiHealthCheck(IAccountsApiClient<AccountsConfiguration> client, ILogger<AccountsApiHealthCheck> logger)
            : base("Accounts API", client, logger)
        {
        }
    }
}