using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<RoatpV2ApiConfiguration>(configuration.GetSection(nameof(RoatpV2ApiConfiguration)));
            services.Configure<RequestApprenticeTrainingApiConfiguration>(configuration.GetSection(nameof(RequestApprenticeTrainingApiConfiguration)));

            services.AddSingleton(cfg => cfg.GetService<IOptions<RoatpV2ApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<RequestApprenticeTrainingApiConfiguration>>().Value);
        }
    }
}