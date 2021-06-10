using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Encoding;

namespace SFA.DAS.LevyTransferMatching.Api.AppStart
{
    public static class AddEncodingServiceExtensions
    {
        public static void AddEncodingService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEncodingService, EncodingService>();
        }
    }
}