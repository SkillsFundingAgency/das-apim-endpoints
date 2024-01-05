using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services
{
    public class WhenGettingCourseLevels
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Levels_Returned_And_Added_To_Cache(
            GetLevelsListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            CourseService service)
        {
            //Arrange
            cacheStorageService.Setup(x => x.RetrieveFromCache<GetLevelsListResponse>(nameof(GetLevelsListResponse)))
                .ReturnsAsync((GetLevelsListResponse)default);
            apiClient.Setup(x => x.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>())).ReturnsAsync(apiResponse);

            //Act
            var actual = await service.GetLevels();

            //Assert
            actual.Should().BeEquivalentTo(apiResponse);
            cacheStorageService.Verify(x=>x.SaveToCache(nameof(GetLevelsListResponse), apiResponse, 23));
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Levels_Are_In_The_Cache_The_Api_Is_Not_Called(
            GetLevelsListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            CourseService service)
        {
            //Arrange
            cacheStorageService.Setup(x => x.RetrieveFromCache<GetLevelsListResponse>(nameof(GetLevelsListResponse)))
                .ReturnsAsync(apiResponse);
        
            //Act
            var actual = await service.GetLevels();

            //Assert
            actual.Should().BeEquivalentTo(apiResponse);
            apiClient.Verify(x => x.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()), Times.Never);
        }
    }
}