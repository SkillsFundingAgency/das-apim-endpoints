using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.EmployerIncentives.Api.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiConfigurationSections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection(EmployerIncentivesConfigurationKeys.AzureActiveDirectoryApiConfiguration));
            return services;
        }
    }
}