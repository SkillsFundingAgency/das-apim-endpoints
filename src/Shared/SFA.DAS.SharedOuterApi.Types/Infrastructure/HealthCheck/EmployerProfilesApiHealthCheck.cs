using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck
{
    public class EmployerProfilesApiHealthCheck : ApiHealthCheck<EmployerProfilesApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Employer Profiles API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

        public EmployerProfilesApiHealthCheck(IEmployerProfilesApiClient<EmployerProfilesApiConfiguration> client, ILogger<EmployerProfilesApiHealthCheck> logger)
            : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
        {
        }
    }
}