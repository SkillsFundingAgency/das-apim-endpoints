using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.FindApprenticeshipTraining.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<CoursesApiConfiguration>(configuration.GetSection(nameof(CoursesApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);
            services.Configure<CourseDeliveryApiConfiguration>(configuration.GetSection(nameof(CourseDeliveryApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CourseDeliveryApiConfiguration>>().Value);
            services.Configure<FindApprenticeshipTrainingConfiguration>(configuration.GetSection(nameof(FindApprenticeshipTrainingConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<FindApprenticeshipTrainingConfiguration>>().Value);
            services.Configure<LocationApiConfiguration>(configuration.GetSection(nameof(LocationApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<LocationApiConfiguration>>().Value);
            services.Configure<ApprenticeFeedbackApiConfiguration>(configuration.GetSection(nameof(ApprenticeFeedbackApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeFeedbackApiConfiguration>>().Value);
            services.Configure<EmployerFeedbackApiConfiguration>(configuration.GetSection(nameof(EmployerFeedbackApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerFeedbackApiConfiguration>>().Value);

            services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
            services.Configure<ShortlistApiConfiguration>(configuration.GetSection(nameof(ShortlistApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<ShortlistApiConfiguration>>().Value);
        }
    }
}