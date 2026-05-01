using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Services;

public class WhenGettingStandardDetails
{
    [Test, MoqAutoData]
    public async Task GetStandardDetails_StandardDetailsFoundInCache_ReturnsCachedStandardDetails(

        StandardDetailsLookupResponse standardDetailsFromCache,
        [Frozen] Mock<ICacheStorageService> mockCacheService,
        CachedStandardDetailsService service)
    {
        var larsCode = "123";
        standardDetailsFromCache.LarsCode = larsCode;

        mockCacheService
            .Setup(service => service.RetrieveFromCache<StandardDetailsLookupResponse>($"{nameof(StandardDetailsLookupResponse)}-{larsCode}"))
            .ReturnsAsync(standardDetailsFromCache);

        var result = await service.GetStandardDetails(larsCode.ToString());

        result.Should().BeEquivalentTo(standardDetailsFromCache);
    }

    [Test, MoqAutoData]
    public async Task GetStandardDetails_StandardDetailsNotFoundInCache_GetsFromApiAndStoresInCache(

        StandardDetailsLookupResponse coursesFromApi,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
        [Frozen] Mock<ICacheStorageService> mockCacheService,
        CachedStandardDetailsService service)
    {
        var expectedExpirationInHours = 4;
        var larsCode = "123";
        coursesFromApi.LarsCode = larsCode;

        mockCoursesApiClient
            .Setup(client => client.GetWithResponseCode<StandardDetailsLookupResponse>(It.IsAny<GetStandardDetailsLookupRequest>()))
            .ReturnsAsync(new ApiResponse<StandardDetailsLookupResponse>(coursesFromApi, System.Net.HttpStatusCode.OK, ""));

        mockCacheService
            .Setup(service => service.RetrieveFromCache<StandardDetailsLookupResponse>($"{nameof(StandardDetailsLookupResponse)}-{larsCode}"))
            .ReturnsAsync((StandardDetailsLookupResponse)null);

        var result = await service.GetStandardDetails(larsCode.ToString());

        result.Should().BeEquivalentTo(coursesFromApi);
        mockCacheService.Verify(service =>
            service.SaveToCache(
                $"{nameof(StandardDetailsLookupResponse)}-{larsCode}",
                coursesFromApi,
                expectedExpirationInHours, null));
    }

    [Test, MoqAutoData]
    public async Task GetKsbsForCourseOption_KsbsFoundInCache_ReturnsCachedKsbs(
        GetKsbsForCourseOptionResponse ksbsFromCache,
        [Frozen] Mock<ICacheStorageService> mockCacheService,
        CachedStandardDetailsService service)
    {
        var larsCode = "123";

        mockCacheService
            .Setup(s => s.RetrieveFromCache<GetKsbsForCourseOptionResponse>($"{nameof(GetKsbsForCourseOptionResponse)}-{larsCode}"))
            .ReturnsAsync(ksbsFromCache);

        var result = await service.GetKsbsForCourseOption(larsCode);

        result.Should().BeEquivalentTo(ksbsFromCache);
    }

    [Test, MoqAutoData]
    public async Task GetKsbsForCourseOption_KsbsNotFoundInCache_GetsFromApiAndStoresInCache(
        GetKsbsForCourseOptionResponse ksbsFromApi,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
        [Frozen] Mock<ICacheStorageService> mockCacheService,
        CachedStandardDetailsService service)
    {
        var expectedExpirationInHours = 4;
        var larsCode = "123";

        mockCoursesApiClient.Setup(client => client.GetWithResponseCode<GetKsbsForCourseOptionResponse>(It.IsAny<GetKsbsForCourseOptionRequest>()))
            .ReturnsAsync(new ApiResponse<GetKsbsForCourseOptionResponse>(ksbsFromApi, System.Net.HttpStatusCode.OK, ""));

        mockCacheService
            .Setup(s => s.RetrieveFromCache<GetKsbsForCourseOptionResponse>($"{nameof(GetKsbsForCourseOptionResponse)}-{larsCode}"))
            .ReturnsAsync((GetKsbsForCourseOptionResponse)null);

        var result = await service.GetKsbsForCourseOption(larsCode);

        result.Should().BeEquivalentTo(ksbsFromApi);
        mockCacheService.Verify(s =>
            s.SaveToCache(
                $"{nameof(GetKsbsForCourseOptionResponse)}-{larsCode}",
                ksbsFromApi,
                expectedExpirationInHours, null));
    }
}