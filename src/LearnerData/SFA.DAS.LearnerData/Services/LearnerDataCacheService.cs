using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Requests;
using System.Text.Json;

namespace SFA.DAS.LearnerData.Services;

public interface ILearnerDataCacheService
{
    Task StoreLearner(UpdateLearnerRequest data, long ukprn, CancellationToken cancellationToken);
    Task<UpdateLearnerRequest?> GetLearner(long ukprn, string uln, CancellationToken cancellationToken);
    Task<List<UpdateLearnerRequest>> GetLearners(long ukprn, IEnumerable<string> ulns, CancellationToken cancellationToken);
}

public class LearnerDataCacheService(IDistributedCache cache, ILogger<LearnerDataCacheService> logger) : ILearnerDataCacheService
{
    private const int CacheDuration = 4;
    private static string BuildKey(long ukprn, string uln) => $"{CacheKeys.LearnerDataPrefix}_{ukprn}_{uln}";

    public async Task StoreLearner(UpdateLearnerRequest data, long ukprn, CancellationToken cancellationToken)
    {
        var key = BuildKey(ukprn, data.Learner.Uln.ToString());
        var json = JsonSerializer.Serialize(data);

        logger.LogInformation("CACHE STORE key={Key} size={Size}B", key, json.Length);

        await cache.SetStringAsync(
            key,
            json,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(CacheDuration)
            },
            cancellationToken);
    }

    public async Task<UpdateLearnerRequest?> GetLearner(long ukprn, string uln, CancellationToken cancellationToken)
    {
        var key = BuildKey(ukprn, uln);

        logger.LogInformation("CACHE GET key={Key}", key);

        var json = await cache.GetStringAsync(key, cancellationToken);

        if (string.IsNullOrEmpty(json))
        {
            logger.LogInformation("CACHE MISS key={Key}", key);
            return null;
        }

        logger.LogInformation("CACHE HIT key={Key} size={Size}B", key, json.Length);

        try
        {
            return JsonSerializer.Deserialize<UpdateLearnerRequest>(json);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CACHE DESERIALIZE FAILED key={Key}", key);
            return null;
        }
    }

    public async Task<List<UpdateLearnerRequest>> GetLearners(long ukprn, IEnumerable<string> ulns, CancellationToken cancellationToken)
    {
        var tasks = ulns.Select(uln => GetLearner(ukprn, uln, cancellationToken));
        var results = await Task.WhenAll(tasks);

        return results.Where(r => r != null).ToList()!;
    }
}