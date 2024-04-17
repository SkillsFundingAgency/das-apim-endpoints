using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.EmployerPR.Api.AppStart;
public static class AddConfigurationOptionsExtension
{
    public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<EmployerProfilesApiConfiguration>(configuration.GetSection(nameof(EmployerProfilesApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerProfilesApiConfiguration>>()!.Value);
        services.Configure<ProviderRelationshipsApiConfiguration>(configuration.GetSection(nameof(ProviderRelationshipsApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderRelationshipsApiConfiguration>>()!.Value);
        services.Configure<AccountsConfiguration>(configuration.GetSection("AccountsInnerApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>()!.Value);
        return services;
    }
}