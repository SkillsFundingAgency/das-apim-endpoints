using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Services;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services;

public class WhenGettingCourseLookupDetailsById
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_With_The_Request_And_CourseDetails_Returned_And_Added_To_Cache(
        CourseLookupDetailResponse apiResponse,
        string courseCode,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        CourseService service)
    {
        //Arrange
        cacheStorageService.Setup(x => x.RetrieveFromCache<CourseLookupDetailResponse>(nameof(CourseLookupDetailResponse) + "_" + courseCode))
            .ReturnsAsync((CourseLookupDetailResponse)default);
        apiClient.Setup(x => x.Get<CourseLookupDetailResponse>(It.Is<GetCourseLookupDetailsByIdRequest>(r => r.Id == courseCode))).ReturnsAsync(apiResponse);

        //Act
        var actual = await service.GetCourseLookupDetailsById(courseCode);

        //Assert
        actual.Should().BeEquivalentTo(apiResponse);
        cacheStorageService.Verify(x => x.SaveToCache(nameof(CourseLookupDetailResponse) + "_" + courseCode, apiResponse, 4, null));
    }

    [Test, MoqAutoData]
    public async Task Then_If_The_CourseDetails_Are_In_The_Cache_The_Api_Is_Not_Called(
        CourseLookupDetailResponse apiResponse,
        string courseCode,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        CourseService service)
    {
        //Arrange
        cacheStorageService.Setup(x => x.RetrieveFromCache<CourseLookupDetailResponse>(nameof(CourseLookupDetailResponse) + "_" + courseCode))
            .ReturnsAsync(apiResponse);

        //Act
        var actual = await service.GetCourseLookupDetailsById(courseCode);

        //Assert
        actual.Should().BeEquivalentTo(apiResponse);
        apiClient.Verify(x => x.Get<CourseLookupDetailResponse>(It.IsAny<GetCourseLookupDetailsByIdRequest>()), Times.Never);
    }
}