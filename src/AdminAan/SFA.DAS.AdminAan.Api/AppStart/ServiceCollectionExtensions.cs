﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using RestEase.HttpClientFactory;
using SFA.DAS.AdminAan.Api.HealthCheck;
using SFA.DAS.AdminAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.AdminAan.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
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
        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetRegionsQuery).Assembly));

        services.AddHttpClient();
        AddAanHubApiClient(services, configuration);
        AddEducationalOrganisationApiClient(services, configuration);
        AddLocationApiClient(services, configuration);
        AddCommitmentsV2ApiClient(services, configuration);
        AddCoursesApiClient(services, configuration);
        AddApprenticeAccountsApiClient(services, configuration);

        services.AddSingleton<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.Configure<LocationApiConfiguration>(configuration.GetSection(nameof(LocationApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<LocationApiConfiguration>>().Value);
        services.AddTransient<ILocationApiClient<LocationApiConfiguration>, LocationApiClient>();
        services.AddTransient<ILocationLookupService, LocationLookupService>();
        services.AddTransient<ICacheStorageService, CacheStorageService>();
        
        if (configuration.IsLocalOrDev())
        {
            services.AddDistributedMemoryCache();
        }
        else
        {
            var aanConfig = configuration.GetSection(nameof(AdminAanConfiguration)).Get<AdminAanConfiguration>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = aanConfig.ApimEndpointsRedisConnectionString;
            });
        }

        return services;
    }

    public static IServiceCollection AddServiceHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<AanHubApiHealthCheck>(AanHubApiHealthCheck.HealthCheckResultDescription,
                failureStatus: HealthStatus.Unhealthy,
                tags: [Ready])
            .AddCheck<CommitmentsV2InnerApiHealthCheck>(CommitmentsV2InnerApiHealthCheck.HealthCheckResultDescription,
                failureStatus: HealthStatus.Unhealthy,
                tags: [Ready])
            .AddCheck<ApprenticeAccountsApiHealthCheck>(ApprenticeAccountsApiHealthCheck.HealthCheckResultDescription,
                failureStatus: HealthStatus.Unhealthy,
                tags: [Ready])
            .AddCheck<CoursesApiHealthCheck>(CoursesApiHealthCheck.HealthCheckResultDescription,
                failureStatus: HealthStatus.Unhealthy,
                tags: [Ready])
            .AddCheck<EducationalOrganisationApiHealthCheck>(EducationalOrganisationApiHealthCheck.HealthCheckResultDescription,
                failureStatus: HealthStatus.Unhealthy,
                tags: [Ready])
            .AddCheck<LocationsApiHealthCheck>(LocationsApiHealthCheck.HealthCheckResultDescription,
                failureStatus: HealthStatus.Unhealthy,
                tags: [Ready]);

        return services;
    }


    private static void AddAanHubApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "AanHubApiConfiguration");

        services.AddRestEaseClient<IAanHubRestApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));
    }
    private static void AddEducationalOrganisationApiClient(IServiceCollection services, IConfigurationRoot configuration)
    {
        services.Configure<EducationalOrganisationApiConfiguration>(configuration.GetSection(nameof(EducationalOrganisationApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<EducationalOrganisationApiConfiguration>>().Value);
        services.AddTransient<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>, EducationalOrganisationApiClient>();
    }

    private static void AddLocationApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "LocationApiConfiguration");

        services.AddRestEaseClient<ILocationApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));
    }

    private static void AddCommitmentsV2ApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "CommitmentsV2ApiConfiguration");

        services.AddRestEaseClient<ICommitmentsV2ApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));
    }

    private static void AddCoursesApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "CoursesApiConfiguration");

        services.AddRestEaseClient<ICoursesApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));
    }

    private static void AddApprenticeAccountsApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "ApprenticeAccountsApiConfiguration");

        services.AddRestEaseClient<IApprenticeAccountsApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));
    }

    private static InnerApiConfiguration GetApiConfiguration(IConfiguration configuration, string configurationName)
        => configuration.GetSection(configurationName).Get<InnerApiConfiguration>();
}
