using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.Types.Configuration;


namespace SFA.DAS.VacanciesManage.Api.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
        services.Configure<AccountsConfiguration>(configuration.GetSection("AccountsInnerApi"));
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<AccountsConfiguration>>().Value);
        services.Configure<ProviderRelationshipsApiConfiguration>(configuration.GetSection("ProviderRelationshipsApi"));
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<ProviderRelationshipsApiConfiguration>>().Value);
        services.Configure<CoursesApiConfiguration>(configuration.GetSection(nameof(CoursesApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<CoursesApiConfiguration>>().Value);
        services.Configure<RoatpV2ApiConfiguration>(configuration.GetSection(nameof(RoatpV2ApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<RoatpV2ApiConfiguration>>().Value);

        services.Configure<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>(configuration.GetSection("RecruitAltApiConfiguration"));
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>>().Value);
    }
}