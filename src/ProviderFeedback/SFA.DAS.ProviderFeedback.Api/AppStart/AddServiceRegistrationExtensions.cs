﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.ProviderFeedback.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.ProviderFeedback.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddServiceRegistrationExtensions
{
    public static void AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddSingleton<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>, ApprenticeFeedbackApiClient>();
        services.AddTransient<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>, EmployerFeedbackApiClient>();
        services.AddTransient<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>, RoatpCourseManagementApiClient>();
        services.AddTransient<IRoatpV2TrainingProviderService, RoatpV2TrainingProviderService>();
        services.AddTransient<ITrainingProviderService, TrainingProviderService>();
        services.AddTransient<IProviderService, ProviderService>();
        services.AddTransient<ICacheStorageService, CacheStorageService>();

        services.Configure<TrainingProviderConfiguration>(configuration.GetSection("TrainingProviderApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<TrainingProviderConfiguration>>().Value);
    }
}

[ExcludeFromCodeCoverage]
public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<ApprenticeFeedbackApiConfiguration>(configuration.GetSection(nameof(ApprenticeFeedbackApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeFeedbackApiConfiguration>>().Value);
        services.Configure<EmployerFeedbackApiConfiguration>(configuration.GetSection(nameof(EmployerFeedbackApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerFeedbackApiConfiguration>>().Value);
        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
        services.Configure<RoatpV2ApiConfiguration>(configuration.GetSection(nameof(RoatpV2ApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<RoatpV2ApiConfiguration>>().Value);
    }
}