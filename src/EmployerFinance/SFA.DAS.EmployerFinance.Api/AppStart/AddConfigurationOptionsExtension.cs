using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.EmployerFinance.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<AccountsConfiguration>(configuration.GetSection("AccountsApiConfiguration"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>().Value);
            services.Configure<CoursesApiConfiguration>(configuration.GetSection(nameof(CoursesApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);
            services.Configure<ProviderCoursesApiConfiguration>(configuration.GetSection(nameof(ProviderCoursesApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderCoursesApiConfiguration>>().Value);
            services.Configure<LevyTransferMatchingApiConfiguration>(configuration.GetSection(nameof(LevyTransferMatchingApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<LevyTransferMatchingApiConfiguration>>().Value);
            services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
            services.Configure<ForecastingApiConfiguration>(configuration.GetSection(nameof(ForecastingApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ForecastingApiConfiguration>>().Value);
            services.Configure<EmployerProfilesApiConfiguration>(configuration.GetSection(nameof(EmployerProfilesApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerProfilesApiConfiguration>>().Value);
        }
    }
}