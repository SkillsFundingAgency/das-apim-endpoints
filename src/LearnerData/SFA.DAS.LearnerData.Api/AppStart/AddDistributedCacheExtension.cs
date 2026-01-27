using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.LearnerData.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddDistributedCacheExtension
{
    public static void AddDistributedCache(this WebApplicationBuilder builder, IConfigurationRoot config)
    {
#if DEBUG
        builder.Services.AddDistributedMemoryCache();
#else
			AddRedisCache(builder, config);
#endif
    }

    private static void AddRedisCache(WebApplicationBuilder builder, IConfigurationRoot config)
    {
        var cacheConfiguration = config.GetSection(nameof(CacheConfiguration)).Get<CacheConfiguration>();

        if(cacheConfiguration == null || string.IsNullOrEmpty(cacheConfiguration.ApimEndpointsRedisConnectionString))
        {
            throw new InvalidOperationException("Redis cache configuration is not set up correctly");
        }

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = cacheConfiguration.ApimEndpointsRedisConnectionString;
        });
    }
}

#pragma warning disable CS8618
public class CacheConfiguration
{
    public string ApimEndpointsRedisConnectionString { get; set; }
}
#pragma warning restore CS8618