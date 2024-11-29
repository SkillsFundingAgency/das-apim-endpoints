﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using RestEase.HttpClientFactory;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.ApprenticeAan.Api.HealthCheck;
using SFA.DAS.ApprenticeAan.Application.Extensions;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.Infrastructure.Configuration;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using ApprenticeAccountsApiHealthCheck = SFA.DAS.ApprenticeAan.Api.HealthCheck.ApprenticeAccountsApiHealthCheck;
using CoursesApiHealthCheck = SFA.DAS.ApprenticeAan.Api.HealthCheck.CoursesApiHealthCheck;

namespace SFA.DAS.ApprenticeAan.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    private static readonly string Ready = "ready";
    public static IServiceCollection AddServiceHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<AanHubApiHealthCheck>(AanHubApiHealthCheck.HealthCheckResultDescription,
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { Ready })
            .AddCheck<CommitmentsV2InnerApiHealthCheck>(CommitmentsV2InnerApiHealthCheck.HealthCheckResultDescription,
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { Ready })
            .AddCheck<ApprenticeAccountsApiHealthCheck>(ApprenticeAccountsApiHealthCheck.HealthCheckResultDescription,
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { Ready })
            .AddCheck<CoursesApiHealthCheck>(CoursesApiHealthCheck.HealthCheckResultDescription,
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { Ready })
            .AddCheck<LocationsApiHealthCheck>(LocationsApiHealthCheck.HealthCheckResultDescription,
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { Ready });
        ;
        return services;
    }

    public static IServiceCollection AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.AddHttpClient();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();

        services.AddApplicationRegistrations();
        AddAanHubApiClient(services, configuration);
        AddCommitmentsV2ApiClient(services, configuration);
        AddApprenticeAccountsApiClient(services, configuration);
        AddCoursesApiClient(services, configuration);
        AddLocationApiClient(services, configuration);

        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
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
            var aanConfig = configuration.GetSection(nameof(ApprenticeAanConfiguration)).Get<ApprenticeAanConfiguration>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = aanConfig.ApimEndpointsRedisConnectionString;
            });
        }

        return services;
    }

    private static void AddAanHubApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "AanHubApiConfiguration");

        services.AddRestEaseClient<IAanHubRestApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));

    }

    private static void AddCommitmentsV2ApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "CommitmentsV2ApiConfiguration");

        services.AddRestEaseClient<ICommitmentsV2InnerApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));
    }

    private static void AddApprenticeAccountsApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "ApprenticeAccountsApiConfiguration");

        services.AddRestEaseClient<IApprenticeAccountsApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));
        
        services.Configure<ApprenticeAccountsApiConfiguration>(configuration.GetSection(nameof(ApprenticeAccountsApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeAccountsApiConfiguration>>().Value);
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>, ApprenticeAccountsApiClient>();
    }

    private static void AddCoursesApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "CoursesApiConfiguration");

        services.AddRestEaseClient<ICoursesApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));
    }

    private static void AddLocationApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "LocationApiConfiguration");

        services.AddRestEaseClient<ILocationApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));
    }

    private static InnerApiConfiguration GetApiConfiguration(IConfiguration configuration, string configurationName)
        => configuration.GetSection(configurationName).Get<InnerApiConfiguration>()!;
}