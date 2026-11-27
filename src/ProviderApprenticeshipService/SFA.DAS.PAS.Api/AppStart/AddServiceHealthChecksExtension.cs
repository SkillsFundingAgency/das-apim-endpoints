using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.PAS.Infrastructure;

namespace SFA.DAS.PAS.Api.AppStart;

public static class AddServiceHealthChecksExtension
{
    private static readonly string[] tags = { "ready" };

    public static IServiceCollection AddServiceHealthChecks(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddCheck<PasApiHealthCheck>("Pas API Health Check", failureStatus: HealthStatus.Unhealthy, tags: tags);

        return services;
    }
}
