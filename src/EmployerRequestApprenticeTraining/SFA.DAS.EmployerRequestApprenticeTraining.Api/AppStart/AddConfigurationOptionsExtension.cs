using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<AccountsConfiguration>(configuration.GetSection("AccountsApiConfiguration"));
            services.Configure<EmployerProfilesApiConfiguration>(configuration.GetSection(nameof(EmployerProfilesApiConfiguration)));
            services.Configure<RequestApprenticeTrainingApiConfiguration>(configuration.GetSection(nameof(RequestApprenticeTrainingApiConfiguration)));

            services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerProfilesApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<RequestApprenticeTrainingApiConfiguration>>().Value);
        }
    }
}