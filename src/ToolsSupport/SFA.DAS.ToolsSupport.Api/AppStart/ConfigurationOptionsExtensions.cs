using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.ToolsSupport.Configuration;

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

        services.Configure<FinanceApiConfiguration>(configuration.GetSection(nameof(FinanceApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<FinanceApiConfiguration>>().Value);
    
        services.Configure<EmployerProfilesApiConfiguration>(configuration.GetSection("EmployerProfilesInnerApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerProfilesApiConfiguration>>().Value);
        
        services.Configure<EmployerUsersApiConfiguration>(configuration.GetSection("EmployerUsersInnerApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerUsersApiConfiguration>>().Value);

        services.Configure<HashingServiceConfiguration>(configuration.GetSection(nameof(HashingServiceConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<HashingServiceConfiguration>>().Value);
        
        services.Configure<TokenServiceApiConfiguration>(configuration.GetSection("TokenServiceApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<TokenServiceApiConfiguration>>().Value);
        
        services.Configure<HmrcApiConfiguration>(configuration.GetSection("HmrcApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<HmrcApiConfiguration>>().Value);
    }
}
