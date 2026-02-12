using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Extensions;

public static class IDistributedCacheExtensions
{
    public static async Task StoreLearner(
        this IDistributedCache cache,
        UpdateLearnerRequest data,
        long ukprn,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var key = $"{CacheKeys.LearnerDataPrefix}_{ukprn}_{data.Learner.Uln}";
        var json = JsonSerializer.Serialize(data);

        logger.LogInformation("CACHE STORE key={Key} json={Json}", key, json);

        await cache.SetStringAsync(
            key,
            json,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4)
            },
            cancellationToken);
    }

    public static async Task<UpdateLearnerRequest?> GetLearner(
        this IDistributedCache cache,
        long ukprn,
        string uln,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var key = $"{CacheKeys.LearnerDataPrefix}_{ukprn}_{uln}";
        logger.LogInformation("CACHE GET key={Key}", key);

        var cachedData = await cache.GetStringAsync(key, cancellationToken);

        if (string.IsNullOrEmpty(cachedData))
        {
            logger.LogInformation("CACHE MISS key={Key}", key);
            return null;
        }

        logger.LogInformation("CACHE HIT key={Key}", key);

        try
        {
            var result = JsonSerializer.Deserialize<UpdateLearnerRequest>(cachedData);
            logger.LogInformation("CACHE DESERIALIZED key={Key} learnerKey={LearnerKey}", key, result?.Learner?.Uln);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CACHE DESERIALIZE FAILED key={Key} json={Json}", key, cachedData);
            return null;
        }
    }

    public static async Task<List<UpdateLearnerRequest>> GetLearners(
        this IDistributedCache cache,
        long ukprn,
        IEnumerable<string> ulns,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var tasks = ulns.Select(uln =>
            GetLearner(cache, ukprn, uln, logger, cancellationToken));

        var results = await Task.WhenAll(tasks);

        return results
            .Where(r => r != null)
            .ToList()!;
    }

}