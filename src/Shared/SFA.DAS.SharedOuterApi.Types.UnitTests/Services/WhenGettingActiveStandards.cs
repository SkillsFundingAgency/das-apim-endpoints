using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Services;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services
{
    public class WhenGettingActiveStandards
    {
        [Test, MoqAutoData]
        public async Task Then_If_The_Standards_Are_Not_Cached_Then_The_Api_Endpoint_Is_Called_And_Results_Cached(
            TestGetStandardsListResponse coursesFromApi,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICacheStorageService> mockCacheService,
            CourseService service)
        {
            mockCacheService.Setup(x => x.RetrieveFromCache<TestGetStandardsListResponse>(nameof(TestGetStandardsListResponse)))
                .ReturnsAsync((TestGetStandardsListResponse)null);
            mockCoursesApiClient.Setup(x => x.Get<TestGetStandardsListResponse>(It.IsAny<GetActiveStandardsListRequest>()))
                .ReturnsAsync(coursesFromApi);

            var actual = await service.GetActiveStandards<TestGetStandardsListResponse>(nameof(TestGetStandardsListResponse));
            
            actual.Should().BeEquivalentTo(coursesFromApi);
            mockCacheService.Verify(x=>x.SaveToCache(nameof(TestGetStandardsListResponse), coursesFromApi, 4, null), Times.Once);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_The_Standards_Are_Cached_Then_Returned_And_Api_Not_Called(
            TestGetStandardsListResponse coursesFromCache,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICacheStorageService> mockCacheService,
            CourseService service)
        {
            mockCacheService.Setup(x => x.RetrieveFromCache<TestGetStandardsListResponse>(nameof(TestGetStandardsListResponse)))
                .ReturnsAsync(coursesFromCache);

            var actual = await service.GetActiveStandards<TestGetStandardsListResponse>(nameof(TestGetStandardsListResponse));

            actual.Should().BeEquivalentTo(coursesFromCache);
            mockCoursesApiClient.Verify(x=>x.Get<TestGetStandardsListResponse>(It.IsAny<GetActiveStandardsListRequest>()), Times.Never);
        }

        public class TestGetStandardsListResponse
        {
            public int Id { get; set; }
            public int Title { get; set; }
        }
    }
}