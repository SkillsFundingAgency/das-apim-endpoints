using System.Net;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
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
        CourseLookupDetailResponse apiResponseBody,
        string courseCode,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        CourseService service)
    {
        //Arrange
        cacheStorageService.Setup(x => x.RetrieveFromCache<CourseLookupDetailResponse>(nameof(CourseLookupDetailResponse) + "_" + courseCode))
            .ReturnsAsync((CourseLookupDetailResponse)default);
        apiClient.Setup(x => x.GetWithResponseCode<CourseLookupDetailResponse>(It.Is<GetCourseLookupDetailsByIdRequest>(r => r.Id == courseCode)))
            .ReturnsAsync(new ApiResponse<CourseLookupDetailResponse>(apiResponseBody, HttpStatusCode.OK, string.Empty));

        //Act
        var actual = await service.GetCourseLookupDetailsById(courseCode);

        //Assert
        actual.Status.Should().Be(CourseLookupStatus.Found);
        actual.Course.Should().BeEquivalentTo(apiResponseBody);
        cacheStorageService.Verify(x => x.SaveToCache(nameof(CourseLookupDetailResponse) + "_" + courseCode, apiResponseBody, 4, null));
    }

    [Test, MoqAutoData]
    public async Task Then_If_The_CourseDetails_Are_In_The_Cache_The_Api_Is_Not_Called(
        CourseLookupDetailResponse apiResponseBody,
        string courseCode,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        CourseService service)
    {
        //Arrange
        cacheStorageService.Setup(x => x.RetrieveFromCache<CourseLookupDetailResponse>(nameof(CourseLookupDetailResponse) + "_" + courseCode))
            .ReturnsAsync(apiResponseBody);

        //Act
        var actual = await service.GetCourseLookupDetailsById(courseCode);

        //Assert
        actual.Status.Should().Be(CourseLookupStatus.Found);
        actual.Course.Should().BeEquivalentTo(apiResponseBody);
        apiClient.Verify(x => x.GetWithResponseCode<CourseLookupDetailResponse>(It.IsAny<GetCourseLookupDetailsByIdRequest>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_A_NotFound_Response_Is_Returned_And_Not_Cached(
        string courseCode,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        CourseService service)
    {
        //Arrange
        cacheStorageService.Setup(x => x.RetrieveFromCache<CourseLookupDetailResponse>(nameof(CourseLookupDetailResponse) + "_" + courseCode))
            .ReturnsAsync((CourseLookupDetailResponse)default);
        apiClient.Setup(x => x.GetWithResponseCode<CourseLookupDetailResponse>(It.Is<GetCourseLookupDetailsByIdRequest>(r => r.Id == courseCode)))
            .ReturnsAsync(new ApiResponse<CourseLookupDetailResponse>(null, HttpStatusCode.NotFound, string.Empty));

        //Act
        var actual = await service.GetCourseLookupDetailsById(courseCode);

        //Assert
        actual.Status.Should().Be(CourseLookupStatus.NotFound);
        cacheStorageService.Verify(x => x.SaveToCache(It.IsAny<string>(), It.IsAny<CourseLookupDetailResponse>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_A_Server_Error_Response_Is_Returned_And_Not_Cached(
        string courseCode,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        CourseService service)
    {
        //Arrange
        cacheStorageService.Setup(x => x.RetrieveFromCache<CourseLookupDetailResponse>(nameof(CourseLookupDetailResponse) + "_" + courseCode))
            .ReturnsAsync((CourseLookupDetailResponse)default);
        apiClient.Setup(x => x.GetWithResponseCode<CourseLookupDetailResponse>(It.Is<GetCourseLookupDetailsByIdRequest>(r => r.Id == courseCode)))
            .ReturnsAsync(new ApiResponse<CourseLookupDetailResponse>(null, HttpStatusCode.InternalServerError, string.Empty));

        //Act
        var actual = await service.GetCourseLookupDetailsById(courseCode);

        //Assert
        actual.Status.Should().Be(CourseLookupStatus.Unavailable);
        cacheStorageService.Verify(x => x.SaveToCache(It.IsAny<string>(), It.IsAny<CourseLookupDetailResponse>(), It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }
}