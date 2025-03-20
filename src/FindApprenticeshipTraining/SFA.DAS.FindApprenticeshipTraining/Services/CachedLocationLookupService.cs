using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Services;

public interface ICachedLocationLookupService
{
    Task<LocationItem> GetCachedLocationInformation(string location, double lat, double lon, bool includeDistrictNameInPostcodeDisplayName = false);
}

public sealed class CachedLocationLookupService : ICachedLocationLookupService
{
    private readonly ICacheStorageService _cacheStorageService;

    private readonly ILocationLookupService _locationLookupService;

    public const int LocationItemCacheExpirationInHours = 1;

    public CachedLocationLookupService(ICacheStorageService cacheStorageService, ILocationLookupService locationLookupService)
    {
        _cacheStorageService = cacheStorageService;
        _locationLookupService = locationLookupService;
    }

    public async Task<LocationItem> GetCachedLocationInformation(string location, double lat, double lon, bool includeDistrictNameInPostcodeDisplayName = false)
    {
        LocationItem locationItem = await _cacheStorageService.RetrieveFromCache<LocationItem>($"loc:{location}");

        if (locationItem is not null)
        {
            return locationItem;
        }

        locationItem = await _locationLookupService.GetLocationInformation(location, 0, 0);

        if (locationItem is not null)
        {
            await _cacheStorageService.SaveToCache($"loc:{location}", locationItem, TimeSpan.FromHours(LocationItemCacheExpirationInHours));
        }

        return locationItem;
    }
}
