using System.Diagnostics.CodeAnalysis;
using MediatR;
using RestEase.HttpClientFactory;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMember;
using SFA.DAS.EmployerAan.Configuration;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.SharedOuterApi.AppStart;

namespace SFA.DAS.EmployerAan.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceRegistration(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddHttpClient();
        services.AddMediatR(typeof(GetEmployerMemberQuery).Assembly);
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();

        AddAanHubApiClient(services, configuration);
        return services;
    }

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
}
