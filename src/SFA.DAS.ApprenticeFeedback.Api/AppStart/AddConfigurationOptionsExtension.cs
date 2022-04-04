using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.ApprenticeFeedback.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.ApprenticeFeedback.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<ApprenticeAccountsApiConfiguration>(configuration.GetSection("ApprenticeAccountsInnerApi"));
            services.Configure<ApprenticeCommitmentsApiConfiguration>(configuration.GetSection("ApprenticeCommitmentsInnerApi"));
            services.Configure<AssessorsApiConfiguration>(configuration.GetSection("AssessorServiceInnerApi"));
            services.Configure<ApprenticeFeedbackApiConfiguration>(configuration.GetSection("ApprenticeFeedbackInnerApi"));
            services.Configure<CoursesApiConfiguration>(configuration.GetSection("CoursesApi"));
            services.Configure<TrainingProviderApiConfiguration>(configuration.GetSection("TrainingProviderApi"));

            services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeAccountsApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeCommitmentsApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<AssessorsApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeFeedbackApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<TrainingProviderApiConfiguration>>().Value);
        }
    }
}