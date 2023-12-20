using System.Diagnostics.CodeAnalysis;
using MediatR;
using RestEase.HttpClientFactory;
using SFA.DAS.AdminAan.Api.Extensions;
using SFA.DAS.AdminAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;

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
        services.AddMediatR(typeof(GetRegionsQuery).Assembly);

        services.AddHttpClient();
        AddAanHubApiClient(services, configuration);
        AddReferenceDataApiClient(services, configuration);
        AddLocationApiClient(services, configuration);

        return services;
    }

    private static void AddAanHubApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "AanHubApiConfiguration");

        services.AddRestEaseClient<IAanHubRestApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));
    }
    private static void AddReferenceDataApiClient(IServiceCollection services, IConfigurationRoot configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "ReferenceDataApiConfiguration");

        services
            .AddRestEaseClient<IReferenceDataApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));
    }

    private static void AddLocationApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "LocationApiConfiguration");

        services.AddRestEaseClient<ILocationApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(), apiConfig.Identifier));
    }

    private static InnerApiConfiguration GetApiConfiguration(IConfiguration configuration, string configurationName)
        => configuration.GetSection(configurationName).Get<InnerApiConfiguration>();
}
