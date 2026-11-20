using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Assessor.Configuration;
using SFA.DAS.Assessor.Contracts.Client;

namespace SFA.DAS.Assessor.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
            services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<AzureActiveDirectoryConfiguration>>().Value);

            services.Configure<AssessorConfiguration>(configuration.GetSection(nameof(AssessorConfiguration)));
            services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<AssessorConfiguration>>().Value);

            services.Configure<AssessorApiConfiguration>(configuration.GetSection(nameof(AssessorApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<AssessorApiConfiguration>>().Value);
        }
    }
}