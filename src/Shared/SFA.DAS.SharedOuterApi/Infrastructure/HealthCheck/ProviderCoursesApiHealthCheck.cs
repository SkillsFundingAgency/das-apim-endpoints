using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class ProviderCoursesApiHealthCheck : ApiHealthCheck<ProviderCoursesApiConfiguration>, IHealthCheck
    {
        public ProviderCoursesApiHealthCheck(IProviderCoursesApiClient<ProviderCoursesApiConfiguration> client, ILogger<ProviderCoursesApiHealthCheck> logger)
            : base("Provider Courses API", client, logger)
        {
        }
    }
}