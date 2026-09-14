using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.SharedOuterApi.Recruit.Services;

public class CachingBankHolidaysService([FromKeyedServices("vanilla")] IBankHolidaysService bankHolidaysService, IMemoryCache cache): IBankHolidaysService
{
    public const string CacheKey = "BankHolidaysData";
    
    public async Task<BankHolidaysData> GetBankHolidayDataAsync(CancellationToken cancellationToken = default)
    {
        return (await cache.GetOrCreateAsync(CacheKey, entry =>
        {
            entry.AbsoluteExpiration = TimeProvider.System.GetUtcNow().AddDays(1).Date;
            return bankHolidaysService.GetBankHolidayDataAsync(cancellationToken);
        }))!;
    }
}