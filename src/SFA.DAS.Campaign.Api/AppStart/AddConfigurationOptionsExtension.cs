using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.Campaign.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<CampaignConfiguration>(configuration.GetSection(nameof(CampaignConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CampaignConfiguration>>().Value);
            services.Configure<CoursesApiConfiguration>(configuration.GetSection(nameof(CoursesApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);
            services.Configure<LocationApiConfiguration>(configuration.GetSection(nameof(LocationApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<LocationApiConfiguration>>().Value);
            services.Configure<FindApprenticeshipApiConfiguration>(configuration.GetSection(nameof(FindApprenticeshipApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<FindApprenticeshipApiConfiguration>>().Value);
            services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
            
            services.Configure<ContentfulApiConfiguration>(configuration.GetSection(nameof(ContentfulApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ContentfulApiConfiguration>>().Value);
            
            services.Configure<ContentfulPreviewApiConfiguration>(configuration.GetSection(nameof(ContentfulPreviewApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ContentfulPreviewApiConfiguration>>().Value);
        }
    }
}