using Microsoft.Extensions.Options;
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
        }
    }
}
