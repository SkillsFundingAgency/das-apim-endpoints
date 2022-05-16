using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services
{
    public class WhenGettingFrameworks
    {
        [Test, MoqAutoData]
        public async Task Then_If_The_Frameworks_Are_Not_Cached_Then_The_Api_Endpoint_Is_Called_And_Results_Cached(
            TestGetFrameworksListResponse frameworksFromApi,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICacheStorageService> mockCacheService,
            CourseService service)
        {
            mockCacheService.Setup(x => x.RetrieveFromCache<TestGetFrameworksListResponse>(nameof(TestGetFrameworksListResponse)))
                .ReturnsAsync((TestGetFrameworksListResponse)null);
            mockCoursesApiClient.Setup(x => x.Get<TestGetFrameworksListResponse>(It.IsAny<GetAllFrameworksRequest>()))
                .ReturnsAsync(frameworksFromApi);

            var actual = await service.GetAllFrameworks<TestGetFrameworksListResponse>(nameof(TestGetFrameworksListResponse));
            
            actual.Should().BeEquivalentTo(frameworksFromApi);
            mockCacheService.Verify(x=>x.SaveToCache(nameof(TestGetFrameworksListResponse), frameworksFromApi, 4), Times.Once);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_The_Frameworks_Are_Cached_Then_Returned_And_Api_Not_Called(
            TestGetFrameworksListResponse frameworksFromCache,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICacheStorageService> mockCacheService,
            CourseService service)
        {
            mockCacheService.Setup(x => x.RetrieveFromCache<TestGetFrameworksListResponse>(nameof(TestGetFrameworksListResponse)))
                .ReturnsAsync(frameworksFromCache);

            var actual = await service.GetAllFrameworks<TestGetFrameworksListResponse>(nameof(TestGetFrameworksListResponse));

            actual.Should().BeEquivalentTo(frameworksFromCache);
            mockCoursesApiClient.Verify(x=>x.Get<TestGetFrameworksListResponse>(It.IsAny<GetAllFrameworksRequest>()), Times.Never);
        }
        public class TestGetFrameworksListResponse
        {
            public int Id { get; set; }
            public int Title { get; set; }
        }
    }
}