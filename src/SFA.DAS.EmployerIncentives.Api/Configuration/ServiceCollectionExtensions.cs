using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerIncentives.Configuration;

namespace SFA.DAS.EmployerIncentives.Api.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiConfigurationSections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<AzureActiveDirectoryApiConfiguration>(configuration.GetSection(EmployerIncentivesConfigurationKeys.AzureActiveDirectoryApiConfiguration));
            return services;
        }
    }
}