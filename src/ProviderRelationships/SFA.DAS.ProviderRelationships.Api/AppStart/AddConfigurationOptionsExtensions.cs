using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.ProviderRelationships.Api.AppStart;

public static class AddConfigurationOptionsExtensions
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>()!.Value);
        services.Configure<AccountsConfiguration>(configuration.GetSection("AccountsInnerApiConfiguration"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>()!.Value);
        services.Configure<EmployerProfilesApiConfiguration>(configuration.GetSection(nameof(EmployerProfilesApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerProfilesApiConfiguration>>()!.Value);
    }
}