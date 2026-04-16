using Microsoft.Extensions.Options;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.Aodp.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
            services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<AzureActiveDirectoryConfiguration>>().Value);

            services.Configure<AodpApiConfiguration>(configuration.GetSection("AodpInnerApiConfiguration"));
            services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<AodpApiConfiguration>>().Value);

            services.Configure<AodpConfiguration>(configuration.GetSection(nameof(AodpConfiguration)));
            services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<AodpConfiguration>>().Value);

            services.Configure<NServiceBusConfiguration>(configuration.GetSection(nameof(NServiceBusConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<NServiceBusConfiguration>>().Value);

            services.Configure<DfeSignInApiConfiguration>(configuration.GetSection(nameof(DfeSignInApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<DfeSignInApiConfiguration>>().Value);


        }
    }
}
