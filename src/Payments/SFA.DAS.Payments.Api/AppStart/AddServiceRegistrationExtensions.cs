using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Payments.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddServiceRegistrationExtensions
{
    public static void AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient(typeof(IAccessTokenApiClient<>), typeof(AccessTokenApiClient<>));
        services.AddTransient<ILearnerDataApiClient<LearnerDataApiConfiguration>, LearnerDataApiClient>();
        services.AddTransient<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>, CollectionCalendarApiClient>();
    }
}

[ExcludeFromCodeCoverage]
public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>()!.Value);

        services.Configure<LearnerDataApiConfiguration>(configuration.GetSection(nameof(LearnerDataApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<LearnerDataApiConfiguration>>()!.Value);

        services.Configure<CollectionCalendarApiConfiguration>(configuration.GetSection(nameof(CollectionCalendarApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<CollectionCalendarApiConfiguration>>()!.Value);
    }

}