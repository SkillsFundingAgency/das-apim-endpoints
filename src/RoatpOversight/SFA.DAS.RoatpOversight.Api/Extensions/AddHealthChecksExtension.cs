using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;

namespace SFA.DAS.RoatpOversight.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class AddHealthChecksExtension
{
    public static IServiceCollection AddDependenciesToHealthChecks(this IHealthChecksBuilder healthChecksBuilder, IServiceCollection services)
    {
        healthChecksBuilder
            .AddCheck<RoatpCourseManagementApiHealthCheck>(RoatpCourseManagementApiHealthCheck.HealthCheckResultDescription,
                        failureStatus: HealthStatus.Unhealthy,
                        tags: new[] { "ready" });
        return services;
    }
}
