using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.EarlyConnect.Api.Extensions;
using SFA.DAS.EarlyConnect.Configuration;
using SFA.DAS.EarlyConnect.Configuration.FeatureToggle;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.EarlyConnect.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddServiceRegistrationExtensions
{
    public static void AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient(typeof(ILepsNeExternalApiClient<>), typeof(LepsNeExternalApiClient<>));
        services.AddTransient<IEarlyConnectApiClient<EarlyConnectApiConfiguration>, EarlyConnectApiClient>();
        services.AddTransient<ILepsNeApiClient<LepsNeApiConfiguration>, LepsNeApiClient>();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddFeatureToggle();
        var asf = configuration.GetSection("Features");
        services.AddFeatureManagement(configuration.GetSection("Features"));
    }
}

[ExcludeFromCodeCoverage]
public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<EarlyConnectApiConfiguration>(configuration.GetSection(nameof(EarlyConnectApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<EarlyConnectApiConfiguration>>().Value);

        services.Configure<LepsNeApiConfiguration>(configuration.GetSection(nameof(LepsNeApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<LepsNeApiConfiguration>>().Value);

        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);

        //services.Configure<FeatureNames>(configuration.GetSection("Features"));
        // services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
    }
}