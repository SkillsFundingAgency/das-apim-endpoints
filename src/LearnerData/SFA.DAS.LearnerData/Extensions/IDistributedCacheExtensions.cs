using Microsoft.Extensions.Caching.Distributed;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Extensions;

public static class IDistributedCacheExtensions
{
    public static async Task StoreLearner(
        this IDistributedCache cache, UpdateLearnerRequest data, long ukprn, CancellationToken cancellationToken)
    {
        await cache.SetStringAsync(
            $"{CacheKeys.LearnerDataPrefix}_{ukprn}_{data.Learner.Uln}",
            System.Text.Json.JsonSerializer.Serialize(data),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4)
            },
            cancellationToken);
    }

    public static async Task<UpdateLearnerRequest?> GetLearner(
        this IDistributedCache cache, long ukprn, string uln, CancellationToken cancellationToken)
    {
        var cachedData = await cache.GetStringAsync(
            $"{CacheKeys.LearnerDataPrefix}_{ukprn}_{uln}",
            cancellationToken);

        if (string.IsNullOrEmpty(cachedData))
        {
            return null;
        }
        return System.Text.Json.JsonSerializer.Deserialize<UpdateLearnerRequest>(cachedData);
    }

    public static async Task<UpdateLearnerRequest?> GetLearner(
        this IDistributedCache cache, long ukprn, long uln, CancellationToken cancellationToken)
    {
        return await GetLearner(cache, ukprn, uln.ToString(), cancellationToken);
    }

    public static async Task<List<UpdateLearnerRequest>> GetLearners(
        this IDistributedCache cache,
        long ukprn,
        IEnumerable<string> ulns,
        CancellationToken cancellationToken)
    {
        var tasks = ulns.Select(uln =>
            GetLearner(cache, ukprn, uln, cancellationToken));

        var results = await Task.WhenAll(tasks);

        return results
            .Where(r => r != null)
            .ToList()!;
    }
}
