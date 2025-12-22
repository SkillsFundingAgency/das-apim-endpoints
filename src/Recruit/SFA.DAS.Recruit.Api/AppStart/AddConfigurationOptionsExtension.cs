using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Recruit.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.Recruit.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<CoursesApiConfiguration>(configuration.GetSection(nameof(CoursesApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);
            services.Configure<RoatpV2ApiConfiguration>(configuration.GetSection(nameof(RoatpV2ApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<RoatpV2ApiConfiguration>>().Value);
            services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
            services.Configure<LocationApiConfiguration>(configuration.GetSection(nameof(LocationApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<LocationApiConfiguration>>().Value);
            services.Configure<AccountsConfiguration>(configuration.GetSection("AccountsInnerApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>().Value);
            services.Configure<RecruitConfiguration>(configuration.GetSection(nameof(RecruitConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<RecruitConfiguration>>().Value);
            services.Configure<EmployerProfilesApiConfiguration>(configuration.GetSection(nameof(EmployerProfilesApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerProfilesApiConfiguration>>().Value);
            services.Configure<CandidateApiConfiguration>(configuration.GetSection(nameof(CandidateApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CandidateApiConfiguration>>().Value);
            services.Configure<RecruitApiConfiguration>(configuration.GetSection("RecruitAltApiConfiguration"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<RecruitApiConfiguration>>().Value);
            services.Configure<BusinessMetricsConfiguration>(configuration.GetSection(nameof(BusinessMetricsConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<BusinessMetricsConfiguration>>().Value);
            services.Configure<RecruitArtificialIntelligenceConfiguration>(configuration.GetSection("RecruitAIApiConfiguration"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<RecruitArtificialIntelligenceConfiguration>>().Value);
        }
    }
}