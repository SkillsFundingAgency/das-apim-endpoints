using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Encoding;

namespace SFA.DAS.DigitalCertificates.Api.AppStart
{
    public static class EncodingServiceRegistrations
    {
        public static IServiceCollection AddEncodingServices(this IServiceCollection services, IConfiguration configuration)
        {
            var dasEncodingConfig = new EncodingConfig { Encodings = [] };
            configuration.GetSection(nameof(dasEncodingConfig.Encodings)).Bind(dasEncodingConfig.Encodings);
            services.AddSingleton(dasEncodingConfig);

            services.AddSingleton<IEncodingService, EncodingService>();
            return services;
        }
    }
}
