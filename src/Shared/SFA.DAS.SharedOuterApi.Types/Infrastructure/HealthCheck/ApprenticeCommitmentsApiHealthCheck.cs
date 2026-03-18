using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck
{
    public class ApprenticeCommitmentsApiHealthCheck : ApiHealthCheck<ApprenticeCommitmentsApiConfiguration>, IHealthCheck
    {
        public static readonly string HealthCheckDescription = "Apprentice Commitments API";
        public static string HealthCheckResultDescription => $"{HealthCheckDescription} check";

        public ApprenticeCommitmentsApiHealthCheck(IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> client, ILogger<ApprenticeCommitmentsApiHealthCheck> logger)
            : base(HealthCheckDescription, HealthCheckResultDescription, client, logger)
        {
        }
    }
}