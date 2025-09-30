using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Recruit.Jobs.Configuration;

namespace SFA.DAS.Recruit.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<RecruitJobsConfiguration>(configuration.GetSection(nameof(RecruitJobsConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<RecruitJobsConfiguration>>().Value);
            
        }
    }
}