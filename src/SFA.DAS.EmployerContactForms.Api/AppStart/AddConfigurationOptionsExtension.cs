using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.EmployerContactForms.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.EmployerContactForms.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<LocationApiConfiguration>(configuration.GetSection(nameof(LocationApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<LocationApiConfiguration>>().Value);
            services.Configure<EmployerContactFormsConfiguration>(configuration.GetSection(nameof(EmployerContactFormsConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerContactFormsConfiguration>>().Value);
            services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
            services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
        }
    }
}