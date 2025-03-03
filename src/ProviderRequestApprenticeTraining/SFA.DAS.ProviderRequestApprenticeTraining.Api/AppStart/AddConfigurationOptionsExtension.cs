using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.ProviderRequestApprenticeTraining.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<RoatpV2ApiConfiguration>(configuration.GetSection(nameof(RoatpV2ApiConfiguration)));
            services.Configure<RequestApprenticeTrainingApiConfiguration>(configuration.GetSection(nameof(RequestApprenticeTrainingApiConfiguration)));
            services.Configure<NServiceBusConfiguration>(configuration.GetSection(nameof(NServiceBusConfiguration)));
            services.Configure<ProviderRequestApprenticeTrainingConfiguration>(configuration.GetSection(nameof(ProviderRequestApprenticeTrainingConfiguration)));

            services.AddSingleton(cfg => cfg.GetService<IOptions<RoatpV2ApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<RequestApprenticeTrainingApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<NServiceBusConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderRequestApprenticeTrainingConfiguration>>().Value);
        }
    }
}