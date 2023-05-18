﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using RestEase.HttpClientFactory;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.ApprenticeAan.Application.Extensions;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.ApprenticeAan.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        AddConfigurationOptions(services, configuration);
        services.AddHttpClient();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient<IAanHubApiClient<AanHubApiConfiguration>, AanHubApiClient>();
        services.AddTransient<ILocationApiClient<LocationApiConfiguration>, LocationApiClient>();
        services.AddTransient<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>, ApprenticeAccountsApiClient>();
        services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
        services.AddApplicationRegistrations();
        AddAanHubApiClient(services, configuration);
        return services;
    }

    private static void AddAanHubApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = configuration
                .GetSection(nameof(AanHubApiConfiguration))
                .Get<AanHubApiConfiguration>();

        services.AddScoped<InnerApiAuthenticationHeaderHandler>();

        services.AddRestEaseClient<IAanHubRestApiClient>(apiConfig.Url)
            .AddHttpMessageHandler<InnerApiAuthenticationHeaderHandler>();
    }


    private static void AddConfigurationOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<AanHubApiConfiguration>(configuration.GetSection(nameof(AanHubApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AanHubApiConfiguration>>()!.Value);
        services.Configure<LocationApiConfiguration>(configuration.GetSection(nameof(LocationApiConfiguration)));
        services.AddSingleton(c => c.GetService<IOptions<LocationApiConfiguration>>()!.Value);
        services.Configure<ApprenticeAccountsApiConfiguration>(configuration.GetSection(nameof(ApprenticeAccountsApiConfiguration)));
        services.AddSingleton(c => c.GetService<IOptions<ApprenticeAccountsApiConfiguration>>()!.Value);
        services.Configure<CoursesApiConfiguration>(configuration.GetSection(nameof(CoursesApiConfiguration)));
        services.AddSingleton(c => c.GetService<IOptions<CoursesApiConfiguration>>()!.Value);
    }
}