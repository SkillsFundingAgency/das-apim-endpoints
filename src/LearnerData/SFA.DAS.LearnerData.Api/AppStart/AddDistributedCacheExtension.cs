using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Apim.Shared.AppStart;
using SFA.DAS.LearnerData.Configuration;

namespace SFA.DAS.LearnerData.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddDistributedCacheExtension
{
    public static void AddDistributedCache(this WebApplicationBuilder builder, IConfigurationRoot config)
    {
        if (config.IsLocalOrDev())
        {
            builder.Services.AddDistributedMemoryCache();
            return;
        }

        AddRedisCache(builder, config);
    }

    private static void AddRedisCache(WebApplicationBuilder builder, IConfigurationRoot config)
    {
        var cacheConfiguration = config.GetSection(nameof(CacheConfiguration)).Get<CacheConfiguration>();

        if (cacheConfiguration is null || string.IsNullOrWhiteSpace(cacheConfiguration.ApimEndpointsRedisConnectionString))
        {
            throw new InvalidOperationException(
                $"{nameof(CacheConfiguration)}:{nameof(CacheConfiguration.ApimEndpointsRedisConnectionString)} must be configured for non-local environments.");
        }

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = cacheConfiguration.ApimEndpointsRedisConnectionString;
        });
    }
}
