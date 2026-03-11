using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Services;

public class WhenGettingStandardDetails
{
    [Test, MoqAutoData]
    public async Task And_StandardDetails_Cached_Then_Gets_StandardDetails_From_Cache(
        StandardDetailResponse standardDetailsFromCache,
        [Frozen] Mock<ICacheStorageService> mockCacheService,
        CachedStandardDetailsService service)
    {
        var larsCode = 123;
        standardDetailsFromCache.LarsCode = larsCode;

        mockCacheService
            .Setup(service => service.RetrieveFromCache<StandardDetailResponse>($"{nameof(StandardDetailResponse)}-{larsCode}"))
            .ReturnsAsync(standardDetailsFromCache);

        var result = await service.GetStandardDetails(larsCode.ToString());

        result.Should().BeEquivalentTo(standardDetailsFromCache);
    }

    [Test, MoqAutoData]
    public async Task And_StandardDetails_Not_Cached_Then_Gets_From_Api_And_Stores_In_Cache(
        StandardDetailResponse coursesFromApi,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
        [Frozen] Mock<ICacheStorageService> mockCacheService,
        CachedStandardDetailsService service)
    {
        var expectedExpirationInHours = 4;
        var larsCode = 123;
        coursesFromApi.LarsCode = larsCode;

        mockCoursesApiClient.Setup(client => client.GetWithResponseCode<StandardDetailResponse>(It.IsAny<GetStandardDetailsByIdRequest>()))
            .ReturnsAsync(new ApiResponse<StandardDetailResponse>(coursesFromApi, System.Net.HttpStatusCode.OK, ""));

        mockCacheService
            .Setup(service => service.RetrieveFromCache<StandardDetailResponse>($"{nameof(StandardDetailResponse)}-{larsCode}"))
            .ReturnsAsync((StandardDetailResponse)null);

        var result = await service.GetStandardDetails(larsCode.ToString());

        result.Should().BeEquivalentTo(coursesFromApi);
        mockCacheService.Verify(service =>
            service.SaveToCache(
                $"{nameof(StandardDetailResponse)}-{larsCode}",
                coursesFromApi,
                expectedExpirationInHours, null));
    }
}
