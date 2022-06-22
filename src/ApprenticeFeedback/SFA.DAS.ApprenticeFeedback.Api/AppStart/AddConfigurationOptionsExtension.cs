using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.ApprenticeFeedback.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<ApprenticeAccountsApiConfiguration>(configuration.GetSection("ApprenticeAccountsInnerApi"));
            services.Configure<AssessorsApiConfiguration>(configuration.GetSection("AssessorServiceInnerApi"));
            services.Configure<ApprenticeFeedbackApiConfiguration>(configuration.GetSection("ApprenticeFeedbackInnerApi"));
                        
            services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeAccountsApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<AssessorsApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeFeedbackApiConfiguration>>().Value);
            
            
        }
    }
}