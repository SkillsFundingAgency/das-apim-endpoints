using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck
{
    public class AccountsApiHealthCheck : ApiHealthCheck<AccountsConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Accounts API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

        public AccountsApiHealthCheck(IAccountsApiClient<AccountsConfiguration> client, ILogger<AccountsApiHealthCheck> logger)
            : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
        {
        }
    }
}