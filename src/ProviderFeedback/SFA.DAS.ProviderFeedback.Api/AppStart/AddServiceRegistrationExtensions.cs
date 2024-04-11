using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.ProviderFeedback.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddServiceRegistrationExtensions
{
    public static void AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient<IProviderFeedbackApiClient<ProviderFeedbackApiConfiguration>, ProviderFeedbackApiClient>();
        services.AddTransient<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>, ApprenticeFeedbackApiClient>();
        services.AddTransient<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>, EmployerFeedbackApiClient>();
    }
}

[ExcludeFromCodeCoverage]
public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<ProviderFeedbackApiConfiguration>(configuration.GetSection(nameof(ProviderFeedbackApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderFeedbackApiConfiguration>>().Value);
        services.Configure<ApprenticeFeedbackApiConfiguration>(configuration.GetSection(nameof(ApprenticeFeedbackApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeFeedbackApiConfiguration>>().Value);
        services.Configure<EmployerFeedbackApiConfiguration>(configuration.GetSection(nameof(EmployerFeedbackApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerFeedbackApiConfiguration>>().Value);
        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
    }
}