using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.ProviderPR.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddConfigurationOptionsExtension
{
    public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<ProviderRelationshipsApiConfiguration>(configuration.GetSection(nameof(ProviderRelationshipsApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderRelationshipsApiConfiguration>>()!.Value);
        services.Configure<RoatpV2ApiConfiguration>(configuration.GetSection(nameof(RoatpV2ApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<RoatpV2ApiConfiguration>>()!.Value);
        services.Configure<AccountsConfiguration>(configuration.GetSection("AccountsInnerApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>()!.Value);
        services.Configure<PensionRegulatorApiConfiguration>(configuration.GetSection("PensionRegulatorApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<PensionRegulatorApiConfiguration>>()!.Value);
        return services;
    }
}
