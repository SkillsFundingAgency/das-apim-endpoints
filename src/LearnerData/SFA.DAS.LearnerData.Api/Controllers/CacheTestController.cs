using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

[ApiController]
[Route("cache-test")]
public class CacheTestController(IDistributedCache cache, ILogger<CacheTestController> logger) : ControllerBase
{
    [HttpGet("write")]
    public async Task<IActionResult> Write(CancellationToken cancellationToken)
    {
        logger.LogInformation("Cache test write");

        var key = "cache_test_key";
        var value = Guid.NewGuid().ToString();

        await cache.SetStringAsync(
            key,
            value,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            },
            cancellationToken);

        return Ok(new { key, value });
    }

    [HttpGet("read")]
    public async Task<IActionResult> Read(CancellationToken cancellationToken)
    {
        logger.LogInformation("Cache test read");

        var key = "cache_test_key";
        var value = await cache.GetStringAsync(key, cancellationToken);

        return Ok(new { key, value });
    }
}