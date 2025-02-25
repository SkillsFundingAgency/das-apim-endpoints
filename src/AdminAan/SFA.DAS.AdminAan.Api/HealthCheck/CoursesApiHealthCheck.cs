using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Api.HealthCheck;

public class CoursesApiHealthCheck(ICoursesApiClient coursesApiClient) : IHealthCheck
{
    public const string HealthCheckResultDescription = "Courses API Health Check";

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        var response = await coursesApiClient.GetHealth(cancellationToken);
        return response.IsSuccessStatusCode
            ? HealthCheckResult.Healthy(HealthCheckResultDescription)
            : HealthCheckResult.Unhealthy(HealthCheckResultDescription);
    }
}