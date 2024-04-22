using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.ApprenticeApp.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<ApprenticeAccountsApiConfiguration>(configuration.GetSection("ApprenticeAccountsInnerApi"));
            services.Configure<ApprenticeCommitmentsApiConfiguration>(configuration.GetSection("ApprenticeCommitmentsInnerApi"));
            services.Configure<TrainingProviderConfiguration>(configuration.GetSection("TrainingProviderApi"));
            services.Configure<CommitmentsV2ApiConfiguration>(configuration.GetSection("CommitmentsV2InnerApi"));
            services.Configure<CoursesApiConfiguration>(configuration.GetSection("CoursesApi"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeAccountsApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeCommitmentsApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<CommitmentsV2ApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<TrainingProviderConfiguration>>().Value);

            services.Configure<NServiceBusConfiguration>(configuration.GetSection(nameof(NServiceBusConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<NServiceBusConfiguration>>().Value);

            services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
        }
    }
}