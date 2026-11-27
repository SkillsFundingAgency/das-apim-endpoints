using RestEase.HttpClientFactory;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.PAS.Infrastructure;

namespace SFA.DAS.PAS.Api.AppStart;

public static class AddApplicationServicesExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddPasApiClient(services, configuration);
        return services;
    }

    private static void AddPasApiClient(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<PasApiHealthCheck>();
        var apiConfig = configuration.GetSection(nameof(PasApiConfiguration)).Get<PasApiConfiguration>()!;

        services.AddRestEaseClient<IPasApiClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(configuration), apiConfig.Identifier));
    }
}
