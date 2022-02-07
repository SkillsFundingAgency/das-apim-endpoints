using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Application.Services;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.UnitTests.Application.Services
{
    public class WhenGettingStandards
    {
        [Test, MoqAutoData]
        public async Task Then_If_The_Standards_Are_Not_Cached_Then_The_Api_Endpoint_Is_Called_And_Results_Cached(
            GetStandardsListResponse coursesFromApi,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICacheStorageService> mockCacheService,
            StandardsService service)
        {
            mockCacheService.Setup(x => x.RetrieveFromCache<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync((GetStandardsListResponse)null);
            mockCoursesApiClient.Setup(x => x.Get<GetStandardsListResponse>(It.IsAny<GetActiveStandardsListRequest>()))
                .ReturnsAsync(coursesFromApi);

            var actual = await service.GetStandards();
            
            actual.Should().BeEquivalentTo(coursesFromApi);
            mockCacheService.Verify(x=>x.SaveToCache(nameof(GetStandardsListResponse), coursesFromApi, 4), Times.Once);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_The_Standards_Are_Cached_Then_Returned_And_Api_Not_Called(
            GetStandardsListResponse coursesFromCache,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICacheStorageService> mockCacheService,
            StandardsService service)
        {
            mockCacheService.Setup(x => x.RetrieveFromCache<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync(coursesFromCache);

            var actual = await service.GetStandards();

            actual.Should().BeEquivalentTo(coursesFromCache);
            mockCoursesApiClient.Verify(x=>x.Get<GetStandardsListResponse>(It.IsAny<GetActiveStandardsListRequest>()), Times.Never);
        }
    }
}