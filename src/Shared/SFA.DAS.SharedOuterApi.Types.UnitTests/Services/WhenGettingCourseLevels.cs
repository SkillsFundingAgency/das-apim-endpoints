using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Services;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services
{
    public class WhenGettingCourseLevels
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Levels_Returned_And_Added_To_Cache(
            GetCourseLevelsListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            CourseService service)
        {
            //Arrange
            cacheStorageService.Setup(x => x.RetrieveFromCache<GetCourseLevelsListResponse>(nameof(GetCourseLevelsListResponse)))
                .ReturnsAsync((GetCourseLevelsListResponse)default);
            apiClient.Setup(x => x.Get<GetCourseLevelsListResponse>(It.IsAny<GetCourseLevelsListRequest>())).ReturnsAsync(apiResponse);

            //Act
            var actual = await service.GetLevels();

            //Assert
            actual.Should().BeEquivalentTo(apiResponse);
            cacheStorageService.Verify(x=>x.SaveToCache(nameof(GetCourseLevelsListResponse), apiResponse, 23, null));
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Levels_Are_In_The_Cache_The_Api_Is_Not_Called(
            GetCourseLevelsListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            CourseService service)
        {
            //Arrange
            cacheStorageService.Setup(x => x.RetrieveFromCache<GetCourseLevelsListResponse>(nameof(GetCourseLevelsListResponse)))
                .ReturnsAsync(apiResponse);
        
            //Act
            var actual = await service.GetLevels();

            //Assert
            actual.Should().BeEquivalentTo(apiResponse);
            apiClient.Verify(x => x.Get<GetCourseLevelsListResponse>(It.IsAny<GetCourseLevelsListRequest>()), Times.Never);
        }
    }
}