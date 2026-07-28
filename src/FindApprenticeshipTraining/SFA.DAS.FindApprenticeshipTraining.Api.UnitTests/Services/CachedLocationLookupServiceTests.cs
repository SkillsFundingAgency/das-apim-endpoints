using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Services;

public sealed class CachedLocationLookupServiceTests
{
    [Test]
    [MoqAutoData]
    public async Task WhenCacheHit_ThenLocationIsReturnedFromTheCache(
        string locationName,
        string authorityName,
        LocationItem locationItem,
        [Frozen] Mock<ICacheStorageService> _cacheStorageServiceMock,
        CachedLocationLookupService sut
    )
    {
        var location = $"{locationName}, {authorityName}";

        _cacheStorageServiceMock
            .Setup(a => a.RetrieveFromCache<LocationItem>($"loc:{location}")
        ).ReturnsAsync(locationItem);

        var result = await sut.GetCachedLocationInformation(location);

        Assert.That(result, Is.Not.Null);

        _cacheStorageServiceMock.Verify(x =>
               x.RetrieveFromCache<LocationItem>($"loc:{location}"
            ), Times.Once);

        _cacheStorageServiceMock.Verify(x =>
            x.SaveToCache(
                $"loc:{location}",
                It.IsAny<LocationItem>(),
                TimeSpan.FromHours(CachedLocationLookupService.LocationItemCacheExpirationInHours), null
            ), Times.Never);
    }

    [Test]
    [MoqAutoData]
    public async Task WhenCacheMiss_ThenLocationIsRetrievedFromLocationApiAndSavedToTheCache(
        string locationName,
        string authorityName,
        LocationItem locationItem,
        [Frozen] Mock<ICacheStorageService> _cacheStorageServiceMock,
        [Frozen] Mock<ILocationLookupService> _locationLookupService,
        CachedLocationLookupService sut
    )
    {
        var location = $"{locationName}, {authorityName}";

        _cacheStorageServiceMock
            .Setup(a => a.RetrieveFromCache<LocationItem>($"loc:{location}")
        ).ReturnsAsync((LocationItem)null);

        _locationLookupService
            .Setup(a => a.GetLocationInformation(location, 0, 0, false)
        ).ReturnsAsync(locationItem);

        var result = await sut.GetCachedLocationInformation(location);

        Assert.That(result, Is.Not.Null);

        _cacheStorageServiceMock.Verify(x =>
               x.RetrieveFromCache<LocationItem>($"loc:{location}"
            ), Times.Once);

        _cacheStorageServiceMock.Verify(x =>
            x.SaveToCache(
                $"loc:{location}",
                It.IsAny<LocationItem>(),
                TimeSpan.FromHours(CachedLocationLookupService.LocationItemCacheExpirationInHours), null
            ), Times.Once);
    }

    [Test]
    [MoqAutoData]
    public async Task WhenCacheMiss_AndNullLocationApiResponse_ThenNullIsReturned(
        string locationName,
        string authorityName,
        LocationItem locationItem,
        [Frozen] Mock<ICacheStorageService> _cacheStorageServiceMock,
        [Frozen] Mock<ILocationLookupService> _locationLookupService,
        CachedLocationLookupService sut
    )
    {
        var location = $"{locationName}, {authorityName}";

        _cacheStorageServiceMock
            .Setup(a => a.RetrieveFromCache<LocationItem>($"loc:{location}")
        ).ReturnsAsync((LocationItem)null);

        _locationLookupService
            .Setup(a => a.GetLocationInformation(location, 0, 0, false)
        ).ReturnsAsync((LocationItem)null);

        var result = await sut.GetCachedLocationInformation(location);

        Assert.That(result, Is.Null);

        _cacheStorageServiceMock.Verify(x =>
               x.RetrieveFromCache<LocationItem>($"loc:{location}"
            ), Times.Once);

        _cacheStorageServiceMock.Verify(x =>
            x.SaveToCache(
                $"loc:{location}",
                It.IsAny<LocationItem>(),
                TimeSpan.FromHours(CachedLocationLookupService.LocationItemCacheExpirationInHours), null
            ), Times.Never);
    }

    [Test]
    [MoqAutoData]
    public async Task WhenLocationIsNull_ThenNullIsReturned(
        [Frozen] Mock<ICacheStorageService> _cacheStorageServiceMock,
        [Frozen] Mock<ILocationLookupService> _locationLookupService,
        CachedLocationLookupService sut
    )
    {
        var result = await sut.GetCachedLocationInformation(string.Empty);

        Assert.That(result, Is.Null);

        _cacheStorageServiceMock.Verify(x =>
            x.RetrieveFromCache<LocationItem>(
                It.IsAny<string>()
            ),
            Times.Never
        );

        _cacheStorageServiceMock.Verify(x =>
            x.SaveToCache(
                It.IsAny<string>(),
                It.IsAny<LocationItem>(),
                TimeSpan.FromHours(CachedLocationLookupService.LocationItemCacheExpirationInHours), null
            ), Times.Never);
    }
}
