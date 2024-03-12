using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Api.HealthCheck;

public class CoursesApiHealthCheck : IHealthCheck
{
    public const string HealthCheckResultDescription = "Courses API Health Check";

    private readonly ICoursesApiClient _coursesApiClient;

    public CoursesApiHealthCheck(ICoursesApiClient coursesApiClient)
    {
        _coursesApiClient = coursesApiClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        var response = await _coursesApiClient.GetHealth(cancellationToken);
        return response.IsSuccessStatusCode
            ? HealthCheckResult.Healthy(HealthCheckResultDescription)
            : HealthCheckResult.Unhealthy(HealthCheckResultDescription);
    }
}
