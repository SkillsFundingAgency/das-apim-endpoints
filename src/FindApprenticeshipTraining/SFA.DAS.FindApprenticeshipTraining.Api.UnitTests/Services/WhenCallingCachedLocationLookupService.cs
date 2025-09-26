using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Services;

public sealed class WhenCallingCachedLocationLookupService
{
    [Test]
    [MoqAutoData]
    public async Task When_Cache_Hit_Then_Location_Is_Returned_From_The_Cache(
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
    public async Task When_Cache_Miss_Then_Location_Is_Retrieved_From_Location_API_And_Saved_To_The_Cache(
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
    public async Task When_Cache_Miss_And_Null_Location_API_Response_Then_Null_Is_Returned(
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
    public async Task When_Location_Is_Null_Then_Null_Is_Returned(
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
