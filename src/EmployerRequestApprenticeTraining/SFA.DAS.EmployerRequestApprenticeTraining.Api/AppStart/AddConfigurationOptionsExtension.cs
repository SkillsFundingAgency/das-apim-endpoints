using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.EmployerRequestApprenticeTraining.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<AccountsConfiguration>(configuration.GetSection("AccountsApiConfiguration"));
            services.Configure<CoursesApiConfiguration>(configuration.GetSection(nameof(CoursesApiConfiguration)));
            services.Configure<LocationApiConfiguration>(configuration.GetSection(nameof(LocationApiConfiguration)));
            services.Configure<EmployerProfilesApiConfiguration>(configuration.GetSection(nameof(EmployerProfilesApiConfiguration)));
            services.Configure<RoatpV2ApiConfiguration>(configuration.GetSection(nameof(RoatpV2ApiConfiguration)));
            services.Configure<RequestApprenticeTrainingApiConfiguration>(configuration.GetSection(nameof(RequestApprenticeTrainingApiConfiguration)));
            services.Configure<NServiceBusConfiguration>(configuration.GetSection(nameof(NServiceBusConfiguration)));
            services.Configure<EmployerRequestApprenticeTrainingConfiguration>(configuration.GetSection(nameof(EmployerRequestApprenticeTrainingConfiguration)));

            services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<LocationApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerProfilesApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<RequestApprenticeTrainingApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<RoatpV2ApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<NServiceBusConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerRequestApprenticeTrainingConfiguration>>().Value);
        }
    }
}