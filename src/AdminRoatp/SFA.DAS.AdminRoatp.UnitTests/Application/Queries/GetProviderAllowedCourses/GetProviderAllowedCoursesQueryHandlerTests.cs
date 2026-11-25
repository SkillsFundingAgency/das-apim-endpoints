using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderAllowedCourses;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetProviderAllowedCourses;

public class GetProviderAllowedCoursesQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_SuccessfulResponse_ReturnsData(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetProviderAllowedCoursesQuery query,
        ProviderAllowedCourseModel allowedCourse,
        GetProviderAllowedCoursesQueryHandler sut)
    {
        // Arrange
        var apiResponse = new GetProviderAllowedCoursesResponse(
            new[] { allowedCourse });

        apiClientMock
            .Setup(a => a.GetWithResponseCode<GetProviderAllowedCoursesResponse>(It.Is<GetProviderAllowedCoursesRequest>(c => c.GetUrl.Equals(new GetProviderAllowedCoursesRequest(query.Ukprn, query.CourseType).GetUrl))))
            .ReturnsAsync(new ApiResponse<GetProviderAllowedCoursesResponse>(apiResponse, HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        apiClientMock.Verify(a =>
            a.GetWithResponseCode<GetProviderAllowedCoursesResponse>(It.Is<GetProviderAllowedCoursesRequest>(c => c.GetUrl.Equals(new GetProviderAllowedCoursesRequest(query.Ukprn, query.CourseType).GetUrl))),
            Times.Once);

        result.AllowedCourses.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(allowedCourse);
    }

    [Test, MoqAutoData]
    public async Task Handle_SuccessfulResponse_ReturnsEmpty(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetProviderAllowedCoursesQuery query,
        GetProviderAllowedCoursesQueryHandler sut)
    {
        // Arrange
        var apiResponse = new GetProviderAllowedCoursesResponse(
            Enumerable.Empty<ProviderAllowedCourseModel>());

        apiClientMock
            .Setup(a => a.GetWithResponseCode<GetProviderAllowedCoursesResponse>(It.IsAny<GetProviderAllowedCoursesRequest>()))
            .ReturnsAsync(new ApiResponse<GetProviderAllowedCoursesResponse>(apiResponse, HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.AllowedCourses.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Handle_UnsuccessfulResponse_ThrowsException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetProviderAllowedCoursesQuery query,
        GetProviderAllowedCoursesQueryHandler sut)
    {
        // Arrange
        var apiResponse =
            new ApiResponse<GetProviderAllowedCoursesResponse>(It.IsAny<GetProviderAllowedCoursesResponse>(), HttpStatusCode.InternalServerError, "");

        apiClientMock
            .Setup(a => a.GetWithResponseCode<GetProviderAllowedCoursesResponse>(It.IsAny<GetProviderAllowedCoursesRequest>()))
            .ReturnsAsync(apiResponse);

        // Act
        Func<Task> result = () => sut.Handle(query, CancellationToken.None);

        // Assert
        await result.Should().ThrowAsync<ApiResponseException>();
    }
}
