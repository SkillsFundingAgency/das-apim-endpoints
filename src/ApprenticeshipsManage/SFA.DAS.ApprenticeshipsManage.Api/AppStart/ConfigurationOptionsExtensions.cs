using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.ApprenticeshipsManage.Configuration;

namespace SFA.DAS.ApprenticeshipsManage.Api.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>()!.Value);

        services.Configure<ApprenticeshipsApiConfiguration>(configuration.GetSection(nameof(ApprenticeshipsApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeshipsApiConfiguration>>()!.Value);

    }
}
