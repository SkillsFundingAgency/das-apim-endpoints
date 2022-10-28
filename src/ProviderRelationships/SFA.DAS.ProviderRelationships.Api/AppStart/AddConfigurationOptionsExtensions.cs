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
        services.Configure<AccountsConfiguration>(configuration.GetSection("AccountsInnerApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>()!.Value);
        services.Configure<EmployerUsersApiConfiguration>(configuration.GetSection(nameof(EmployerUsersApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerUsersApiConfiguration>>()!.Value);
    }
}