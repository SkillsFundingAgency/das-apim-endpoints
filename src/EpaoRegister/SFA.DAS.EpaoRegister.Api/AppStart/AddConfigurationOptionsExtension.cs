using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.EpaoRegister.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.EpaoRegister.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<EpaoRegisterConfiguration>(configuration.GetSection(nameof(EpaoRegisterConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<EpaoRegisterConfiguration>>().Value);
            services.Configure<CoursesApiConfiguration>(configuration.GetSection(nameof(CoursesApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);
            services.Configure<AssessorsApiConfiguration>(configuration.GetSection(nameof(AssessorsApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AssessorsApiConfiguration>>().Value);
            services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
        }
    }
}