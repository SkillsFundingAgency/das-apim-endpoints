using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.FindAnApprenticeship.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.FindAnApprenticeship.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<LocationApiConfiguration>(configuration.GetSection(nameof(LocationApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<LocationApiConfiguration>>().Value);
            services.Configure<FindApprenticeshipApiConfiguration>(configuration.GetSection(nameof(FindApprenticeshipApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<FindApprenticeshipApiConfiguration>>().Value);
            services.Configure<FindAnApprenticeshipConfiguration>(configuration.GetSection(nameof(FindAnApprenticeshipConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<FindAnApprenticeshipConfiguration>>().Value);
            services.Configure<CoursesApiConfiguration>(configuration.GetSection(nameof(CoursesApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);
            services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
        }
    }
}