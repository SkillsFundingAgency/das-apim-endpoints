using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class ApprenticeAccountsApiHealthCheck : ApiHealthCheck<ApprenticeAccountsApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Apprentice Accounts API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

        public ApprenticeAccountsApiHealthCheck(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> client, ILogger<ApprenticeAccountsApiHealthCheck> logger)
            : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
        {
        }
    }
}