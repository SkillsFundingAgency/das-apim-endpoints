using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class AccountsApiHealthCheck : ApiHealthCheck<AccountsConfiguration>, IHealthCheck
    {
        public const string AccountsApiHealthCheckDescription = "Accounts API";
        
        public AccountsApiHealthCheck(IAccountsApiClient<AccountsConfiguration> client, ILogger<AccountsApiHealthCheck> logger)
            : base(AccountsApiHealthCheckDescription, client, logger)
        {
        }
    }
}