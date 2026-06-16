using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Admin.Configuration;
using SFA.DAS.DigitalCertificates.Contracts.Client;

namespace SFA.DAS.Admin.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<DigitalCertificatesApiConfiguration>(configuration.GetSection(nameof(DigitalCertificatesApiConfiguration)));
            services.Configure<AdminConfiguration>(configuration.GetSection(nameof(AdminConfiguration)));

            services.AddSingleton(cfg => cfg.GetService<IOptions<DigitalCertificatesApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<AdminConfiguration>>().Value);
        }
    }
}