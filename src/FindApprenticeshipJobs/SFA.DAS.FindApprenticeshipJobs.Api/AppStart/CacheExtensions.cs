using System.Diagnostics.CodeAnalysis;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.SharedOuterApi.AppStart;

namespace SFA.DAS.FindApprenticeshipJobs.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class CacheExtensions
    {
        public static IServiceCollection AddCache(this IServiceCollection services, IConfigurationRoot configuration)
        {
            if (configuration.IsLocalOrDev())
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddStackExchangeRedisCache((options) =>
                {
                    var azureAdConfiguration = configuration
                        .GetSection(nameof(FindApprenticeshipJobsConfiguration))
                        .Get<FindApprenticeshipJobsConfiguration>();

                    options.Configuration = azureAdConfiguration.ApimEndpointsRedisConnectionString;
                });
            }

            return services;
        }
    }
}
