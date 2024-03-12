using Microsoft.Extensions.Diagnostics.HealthChecks;
using RestEase.HttpClientFactory;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.RoatpProviderModeration.Api.AppStart;
using SFA.DAS.RoatpProviderModeration.Api.HealthCheck;
using SFA.DAS.RoatpProviderModeration.Application.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.RoatpProviderModeration.OuterApi.AppStart;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        AddRoatpV2ApiClient(services, configuration);
        return services;
    }

    public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        return services;
    }

    private static void AddRoatpV2ApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "RoatpV2ApiConfiguration");
        services.AddRestEaseClient<IRoatpV2ApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));
    }

    public static IServiceCollection AddServiceHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<RoatpV2ApiHealthCheck>(RoatpV2ApiHealthCheck.HealthCheckResultDescription,
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "ready" });

        return services;
    }

    private static InnerApiConfiguration GetApiConfiguration(IConfiguration configuration, string configurationName)
        => configuration.GetSection(configurationName).Get<InnerApiConfiguration>()!;
}