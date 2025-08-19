using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;

namespace SFA.DAS.AdminRoatp.Api.AppStart;

public static class AddServiceHealthChecksExtension
{
    private static readonly string[] tags = { "ready" };

    public static IServiceCollection AddServiceHealthChecks(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddCheck<RoatpApiHealthCheck>(RoatpApiHealthCheck.HealthCheckResultDescription, failureStatus: HealthStatus.Unhealthy, tags: tags);

        return services;
    }
}
