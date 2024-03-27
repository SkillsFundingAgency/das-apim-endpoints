using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class EmployerProfilesApiHealthCheck : ApiHealthCheck<EmployerProfilesApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Employer Profiles API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

        public EmployerProfilesApiHealthCheck(IEmployerProfilesApiClient<EmployerProfilesApiConfiguration> client, ILogger<EmployerProfilesApiHealthCheck> logger)
            : base(HealthCheckDescription, client, logger)
        {
        }
    }
}