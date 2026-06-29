using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetAllRestrictedCourses;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetAllRestrictedCourses;

public class GetAllRestrictedCoursesQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task WhenHandlingRequest_ThenReturnsResults(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetAllRestrictedCoursesResponse response,
            GetAllRestrictedCoursesQuery query,
            GetAllRestrictedCoursesQueryHandler sut)
    {
        // Arrange
        apiClientMock.Setup(c => c.GetWithResponseCode<GetAllRestrictedCoursesResponse>(It.IsAny<GetAllRestrictedCoursesRequest>())).ReturnsAsync(new ApiResponse<GetAllRestrictedCoursesResponse>(response, HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, new CancellationToken());

        // Assert
        result.Should().BeEquivalentTo(response);
    }

    [Test, MoqAutoData]
    public async Task WhenHandlingRequestAndApiReturnsEmptyResponse_ThenReturnsEmptySet(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetAllRestrictedCoursesQuery query,
        GetAllRestrictedCoursesQueryHandler sut)
    {
        // Arrange
        var response = new GetAllRestrictedCoursesResponse();
        apiClientMock.Setup(c => c.GetWithResponseCode<GetAllRestrictedCoursesResponse>(It.IsAny<GetAllRestrictedCoursesRequest>())).ReturnsAsync(new ApiResponse<GetAllRestrictedCoursesResponse>(response, HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, new CancellationToken());

        // Assert
        result.Courses.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public void WhenHandlingRequestAndApiReturnsInvalidResponseCode_ThenThrowsApiResponseException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetAllRestrictedCoursesQuery query,
        GetAllRestrictedCoursesQueryHandler sut)
    {
        // Arrange
        apiClientMock.Setup(c => c.GetWithResponseCode<GetAllRestrictedCoursesResponse>(It.IsAny<GetAllRestrictedCoursesRequest>())).ReturnsAsync(new ApiResponse<GetAllRestrictedCoursesResponse>(new GetAllRestrictedCoursesResponse(), HttpStatusCode.InternalServerError, ""));

        // Act & Assert
        Assert.ThrowsAsync<ApiResponseException>(() => sut.Handle(query, new CancellationToken()));
    }
}
