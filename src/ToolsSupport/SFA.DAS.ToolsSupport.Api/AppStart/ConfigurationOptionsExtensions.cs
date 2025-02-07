using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.ToolsSupport.Api.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);

        services.Configure<CommitmentsV2ApiConfiguration>(configuration.GetSection("CommitmentsV2InnerApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<CommitmentsV2ApiConfiguration>>().Value);

        services.Configure<AccountsConfiguration>(configuration.GetSection("AccountsInnerApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>().Value);
    }
}
