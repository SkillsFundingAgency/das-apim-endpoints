using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.DigitalCertificates.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.DigitalCertificates.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<AssessorsApiConfiguration>(configuration.GetSection(nameof(AssessorsApiConfiguration)));
            services.Configure<DigitalCertificatesApiConfiguration>(configuration.GetSection(nameof(DigitalCertificatesApiConfiguration)));
            services.Configure<NServiceBusConfiguration>(configuration.GetSection(nameof(NServiceBusConfiguration)));
            services.Configure<DigitalCertificatesConfiguration>(configuration.GetSection(nameof(DigitalCertificatesConfiguration)));

            services.AddSingleton(cfg => cfg.GetService<IOptions<AssessorsApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<DigitalCertificatesApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<NServiceBusConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<DigitalCertificatesConfiguration>>().Value);
        }
    }
}