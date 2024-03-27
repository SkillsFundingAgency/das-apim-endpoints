using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class CoursesApiHealthCheck : ApiHealthCheck<CoursesApiConfiguration>, IHealthCheck
    {
        public CoursesApiHealthCheck(ICoursesApiClient<CoursesApiConfiguration> client, ILogger<CoursesApiHealthCheck> logger)
            : base("Courses API", client, logger)
        {
        }
    }
}