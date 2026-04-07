using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Services
{
    public class WhenGettingCourses
    {
        [Test, MoqAutoData]
        public async Task GetCourses_CoursesCached_ReturnsCoursesFromCache(
            GetStandardsListResponse coursesFromCache,
            [Frozen] Mock<ICacheStorageService> mockCacheService,
            CachedCoursesService service)
        {
            mockCacheService
                .Setup(service => service.RetrieveFromCache<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync(coursesFromCache);

            var result = await service.GetCourses();

            result.Should().BeEquivalentTo(coursesFromCache);
        }

        [Test, MoqAutoData]
        public async Task GetCourses_CoursesNotCached_GetsFromApiAndStoresInCache(
            GetStandardsListResponse coursesFromApi,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICacheStorageService> mockCacheService,
            CachedCoursesService service)
        {
            var expectedExpirationInHours = 1;
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.IsAny<GetActiveStandardsSearchRequest>()))
                .ReturnsAsync(coursesFromApi);
            mockCacheService
                .Setup(service => service.RetrieveFromCache<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync((GetStandardsListResponse)null);

            var result = await service.GetCourses();

            result.Should().BeEquivalentTo(coursesFromApi);
            mockCacheService.Verify(service =>
                service.SaveToCache(
                    nameof(GetStandardsListResponse),
                    coursesFromApi,
                    expectedExpirationInHours, null));
        }
    }
}
