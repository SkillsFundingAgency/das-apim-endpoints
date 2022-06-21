using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.VacanciesManage.Application.TrainingCourses.Queries;
using SFA.DAS.VacanciesManage.InnerApi.Responses;

namespace SFA.DAS.VacanciesManage.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenHandlingGetTrainingCourses
    {
        [Test, MoqAutoData]
        public async Task And_Courses_Cached_Then_Gets_Courses_From_Cache(
            GetTrainingCoursesQuery query,
            GetStandardsListResponse coursesFromCache,
            [Frozen] Mock<ICacheStorageService> mockCacheService,
            GetTrainingCoursesQueryHandler handler)
        {
            mockCacheService
                .Setup(service => service.RetrieveFromCache<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync(coursesFromCache);

            var result = await handler.Handle(query, CancellationToken.None);

            result.TrainingCourses.Should().BeEquivalentTo(coursesFromCache.Standards);
        }

        [Test, MoqAutoData]
        public async Task And_Courses_Not_Cached_Then_Gets_From_Api_And_Stores_In_Cache(
            GetTrainingCoursesQuery query,
            GetStandardsListResponse coursesFromApi,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICacheStorageService> mockCacheService,
            GetTrainingCoursesQueryHandler handler)
        {
            var expectedExpirationInHours = 3;
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.IsAny<GetActiveStandardsListRequest>()))
                .ReturnsAsync(coursesFromApi);
            mockCacheService
                .Setup(service => service.RetrieveFromCache<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync((GetStandardsListResponse)null);

            var result = await handler.Handle(query, CancellationToken.None);

            result.TrainingCourses.Should().BeEquivalentTo(coursesFromApi.Standards);
            mockCacheService.Verify(service =>
                service.SaveToCache(
                    nameof(GetStandardsListResponse),
                    coursesFromApi,
                    expectedExpirationInHours));
        }
    }
}