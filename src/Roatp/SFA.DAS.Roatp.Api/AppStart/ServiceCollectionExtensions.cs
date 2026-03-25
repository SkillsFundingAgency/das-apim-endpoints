using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using RestEase.HttpClientFactory;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Roatp.Api.HealthCheck;
using SFA.DAS.Roatp.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.Ukrlp;

namespace SFA.DAS.Roatp.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static void AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddSingleton<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        AddUkrlpClient(services, configuration);
        AddCharityApiClient(services, configuration);
        AddRoatpApiClient(services, configuration);
    }

    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
    }

    private static InnerApiConfiguration GetApiConfiguration(IConfiguration configuration, string configurationName)
        => configuration.GetSection(configurationName).Get<InnerApiConfiguration>()!;

    private static void AddCharityApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "CharitiesApiConfiguration");
        services.AddRestEaseClient<ICharitiesRestApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));
    }

    private static void AddRoatpApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "RoatpApiConfiguration");
        services.AddRestEaseClient<IRoatpApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(configuration), apiConfig.Identifier));
    }

    private static void AddUkrlpClient(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IUkrlpSoapSerializer, UkrlpSoapSerializer>();
        services.Configure<UkrlpApiConfiguration>(configuration.GetSection(nameof(UkrlpApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<UkrlpApiConfiguration>>().Value);
    }

    public static IServiceCollection AddServiceHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<CharitiesApiHealthCheck>(CharitiesApiHealthCheck.HealthCheckResultDescription,
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "ready" });

        return services;
    }
}
