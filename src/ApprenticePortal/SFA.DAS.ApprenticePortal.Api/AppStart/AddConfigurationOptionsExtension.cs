using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.ApprenticePortal.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<ApprenticeAccountsApiConfiguration>(configuration.GetSection("ApprenticeAccountsInnerApi"));
            services.Configure<ApprenticeCommitmentsApiConfiguration>(configuration.GetSection("ApprenticeCommitmentsInnerApi"));
            services.Configure<ProviderAccountApiConfiguration>(configuration.GetSection("TrainingProviderInnerApi"));
            services.Configure<CommitmentsV2ApiConfiguration>(configuration.GetSection("CommitmentsV2InnerApi"));
            services.Configure<CoursesApiConfiguration>(configuration.GetSection("CoursesInnerApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeAccountsApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeCommitmentsApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderAccountApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<CommitmentsV2ApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);

            services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
        }
    }
}