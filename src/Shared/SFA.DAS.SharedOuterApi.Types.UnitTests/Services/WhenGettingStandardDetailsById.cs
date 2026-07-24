using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Services;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services;

public class WhenGettingStandardLookupDetailsById
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_StandardDetails_Returned_And_Added_To_Cache(
        StandardDetailResponse apiResponse,
        string standardCode,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        CourseService service)
    {
        //Arrange
        cacheStorageService.Setup(x => x.RetrieveFromCache<StandardDetailResponse>(nameof(StandardDetailResponse) + "_" + standardCode))
            .ReturnsAsync((StandardDetailResponse)default);
        apiClient.Setup(x => x.Get<StandardDetailResponse>(It.Is<GetStandardDetailsByIdRequest>(r => r.Id == standardCode))).ReturnsAsync(apiResponse);

        //Act
        var actual = await service.GetStandardDetailsById(standardCode);

        //Assert
        actual.Should().BeEquivalentTo(apiResponse);
        cacheStorageService.Verify(x => x.SaveToCache(nameof(StandardDetailResponse) + "_" + standardCode, apiResponse, 4, null));
    }

    [Test, MoqAutoData]
    public async Task Then_If_The_StandardDetails_Are_In_The_Cache_The_Api_Is_Not_Called(
        StandardDetailResponse apiResponse,
        string standardCode,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        CourseService service)
    {
        //Arrange
        cacheStorageService.Setup(x => x.RetrieveFromCache<StandardDetailResponse>(nameof(StandardDetailResponse) + "_" + standardCode))
            .ReturnsAsync(apiResponse);

        //Act
        var actual = await service.GetStandardDetailsById(standardCode);

        //Assert
        actual.Should().BeEquivalentTo(apiResponse);
        apiClient.Verify(x => x.Get<StandardDetailResponse>(It.IsAny<GetStandardDetailsByIdRequest>()), Times.Never);
    }
}