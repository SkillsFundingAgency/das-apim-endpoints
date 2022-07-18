using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.ApimDeveloper.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
            services.Configure<AccountsConfiguration>(configuration.GetSection("AccountsInnerApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>().Value);
            services.Configure<ApimDeveloperApiConfiguration>(configuration.GetSection(nameof(ApimDeveloperApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ApimDeveloperApiConfiguration>>().Value);
            services.Configure<ApimDeveloperMessagingConfiguration>(configuration.GetSection(nameof(ApimDeveloperMessagingConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ApimDeveloperMessagingConfiguration>>().Value);
            services.Configure<NServiceBusConfiguration>(configuration.GetSection(nameof(NServiceBusConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<NServiceBusConfiguration>>().Value);
        }
    }
}