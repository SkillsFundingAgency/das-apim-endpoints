using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class ApprenticeCommitmentsApiHealthCheck : ApiHealthCheck<ApprenticeCommitmentsApiConfiguration>, IHealthCheck
    {
        public ApprenticeCommitmentsApiHealthCheck(IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> client, ILogger<ApprenticeCommitmentsApiHealthCheck> logger)
            : base("Apprentice Commitments API", client, logger)
        {
        }
    }
}