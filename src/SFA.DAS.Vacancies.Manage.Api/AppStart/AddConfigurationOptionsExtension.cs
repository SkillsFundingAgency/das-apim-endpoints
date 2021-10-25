using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.Vacancies.Manage.Configuration;

namespace SFA.DAS.Vacancies.Manage.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
            services.Configure<RecruitApiConfiguration>(configuration.GetSection(nameof(RecruitApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<RecruitApiConfiguration>>().Value);
            services.Configure<AccountsConfiguration>(configuration.GetSection("AccountsInnerApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>().Value);
            services.Configure<ProviderRelationshipsApiConfiguration>(configuration.GetSection("ProviderRelationshipsApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderRelationshipsApiConfiguration>>().Value);
            services.Configure<CoursesApiConfiguration>(configuration.GetSection(nameof(CoursesApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);
        }
    }
}