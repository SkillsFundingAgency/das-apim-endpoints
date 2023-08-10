using MediatR;
using Microsoft.Extensions.Options;
using RestEase.HttpClientFactory;
using SFA.DAS.AdminAan.Api.Configuration;
using SFA.DAS.AdminAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.AppStart;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AdminAan.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
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
        AddConfigurationOptions(services, configuration);
        services.AddHttpClient();

        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient<ILocationApiClient<LocationApiConfiguration>, LocationApiClient>();

        services.AddMediatR(typeof(GetRegionsQuery).Assembly);
        
        AddAanHubApiClient(services, configuration);

        return services;
    }

    private static void AddAanHubApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = configuration
                .GetSection(nameof(AanHubApiConfiguration))
                .Get<AanHubApiConfiguration>();

        services.AddSingleton(apiConfig);

        services.AddScoped<InnerApiAuthenticationHeaderHandler>();

        services.AddRestEaseClient<IAanHubRestApiClient>(apiConfig.Url)
            .AddHttpMessageHandler<InnerApiAuthenticationHeaderHandler>();
    }

    private static void AddConfigurationOptions(IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddOptions();
        services.Configure<AanHubApiConfiguration>(configuration.GetSection(nameof(AanHubApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AanHubApiConfiguration>>()!.Value);
        services.Configure<LocationApiConfiguration>(configuration.GetSection(nameof(LocationApiConfiguration)));
        services.AddSingleton(c => c.GetService<IOptions<LocationApiConfiguration>>()!.Value);
    }
}