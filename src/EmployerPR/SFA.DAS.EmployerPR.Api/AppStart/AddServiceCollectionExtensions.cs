using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.EmployerPR.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddServiceCollectionExtensions
{
    private static readonly string Ready = "ready";
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfigurationRoot configuration)
    {
        if (configuration.IsLocalAcceptanceTestsOrDev() || configuration.IsLocal()) return services;

        var azureAdConfiguration = configuration
            .GetSection("AzureAd")
            .Get<AzureActiveDirectoryConfiguration>();
        var policies = new Dictionary<string, string>
        {
            {"default", "APIM"}
        };

        services.AddAuthentication(azureAdConfiguration, policies);

        return services;
    }

    public static IServiceCollection AddServiceRegistration(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddHttpClient();

        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
        services.AddTransient<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>, EmployerProfilesApiClient>();
        services.AddTransient<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>, ProviderRelationshipsApiClient>();

        services.AddTransient<IEmployerAccountsService, EmployerAccountsService>();
        return services;
    }

    public static IServiceCollection AddServiceHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<AccountsApiHealthCheck>(AccountsApiHealthCheck.HealthCheckResultDescription,
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { Ready })
        .AddCheck<EmployerProfilesApiHealthCheck>(EmployerProfilesApiHealthCheck.HealthCheckResultDescription,
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { Ready });

        // NOTE: The employer profiles inner api does not seem to work to provide a standard health check when running locally
        // However there is evidence this is working when deployed (the outer api for EmployerRequestApprenticeTraining has it)

        // TODO add a healthcheck for ProviderRelationships.  Do we add this to shared?
        return services;
    }
}