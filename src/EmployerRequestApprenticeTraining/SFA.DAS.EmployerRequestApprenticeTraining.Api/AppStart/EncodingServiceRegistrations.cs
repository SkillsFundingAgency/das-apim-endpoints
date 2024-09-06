using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.AppStart
{
    public static class EncodingServiceRegistrations
    {
        public static IServiceCollection AddEncodingServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEncodingService, EncodingService>();
            return services;
        }
    }
}
