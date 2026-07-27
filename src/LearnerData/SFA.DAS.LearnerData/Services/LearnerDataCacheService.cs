using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerData.Requests;
using System.Text.Json;

namespace SFA.DAS.LearnerData.Services;

public interface ILearnerDataCacheService
{
    Task StoreLearner<T>(T data, long ukprn, CancellationToken cancellationToken);
    Task<T?> GetLearner<T>(long ukprn, string uln, CancellationToken cancellationToken) where T : class;
    Task<List<T>> GetLearners<T>(long ukprn, IEnumerable<string> ulns, CancellationToken cancellationToken) where T : class;
}

public class LearnerDataCacheService(IDistributedCache cache, ILogger<LearnerDataCacheService> logger) : ILearnerDataCacheService
{
    private const string ApprenticeshipLearnerDataPrefix = "LearnerDataApprenticeship";
    private const string ShortCourseLearnerDataPrefix = "LearnerDataShortCourse";

    private const int CacheDuration = 4;
    private static string BuildKey(string prefix, long ukprn, string uln) => $"{prefix}_{ukprn}_{uln}";

    public async Task StoreLearner<T>(T data, long ukprn, CancellationToken cancellationToken)
    {
        var key = GetKey(data, ukprn);
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

    public async Task<T?> GetLearner<T>(long ukprn, string uln, CancellationToken cancellationToken) where T : class
    {
        var key = GetKey<T>(ukprn, uln);

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
            return JsonSerializer.Deserialize<T>(json);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "CACHE DESERIALIZE FAILED key={Key}", key);
            return null;
        }
    }

    public async Task<List<T>> GetLearners<T>(long ukprn, IEnumerable<string> ulns, CancellationToken cancellationToken) where T : class
    {
        var tasks = ulns.Select(uln => GetLearner<T>(ukprn, uln, cancellationToken));
        var results = await Task.WhenAll(tasks);

        return results.Where(r => r != null).ToList()!;
    }

    private string GetKey<T>(T data, long ukprn) 
    {
        if(data is UpdateLearnerRequest learnerData)
        {
            return BuildKey(ApprenticeshipLearnerDataPrefix, ukprn, learnerData.Learner.Uln.ToString());
        }

        if(data is ShortCourseRequest shortCourseData)
        {
            return BuildKey(ShortCourseLearnerDataPrefix, ukprn, shortCourseData.Learner.Uln.ToString());
        }

        throw new ArgumentException("Unsupported data type", nameof(data));
    }

    private string GetKey<T>(long ukprn, string uln)
    {
        if (typeof(T) == typeof(UpdateLearnerRequest))
        {
            return BuildKey(ApprenticeshipLearnerDataPrefix, ukprn, uln);
        }

        if (typeof(T) == typeof(ShortCourseRequest))
        {
            return BuildKey(ShortCourseLearnerDataPrefix, ukprn, uln);
        }

        throw new ArgumentException("Unsupported data type", nameof(T));
    }
}