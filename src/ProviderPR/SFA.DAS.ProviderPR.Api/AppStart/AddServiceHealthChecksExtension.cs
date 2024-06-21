using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.ProviderPR.Api.HealthCheck;

namespace SFA.DAS.ProviderPR.Api.AppStart;

public static class AddServiceHealthChecksExtension
{
    private static readonly string[] tags = { "ready" };

    public static IServiceCollection AddServiceHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<ProviderRelationshipsApiHealthCheck>(ProviderRelationshipsApiHealthCheck.HealthCheckResultDescription,
                failureStatus: HealthStatus.Unhealthy,
                tags: tags);

        return services;
    }
}
