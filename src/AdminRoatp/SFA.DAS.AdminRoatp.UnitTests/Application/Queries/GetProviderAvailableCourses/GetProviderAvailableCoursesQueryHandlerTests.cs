using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderAvailableCourses;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetProviderAvailableCourses;

public class GetProviderAvailableCoursesQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task WhenStandardsAndProviderAllowedCoursesReturnsCourses_ThenRetrunsCoursesNotInProviderAllowedCourses(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] GetProviderAvailableCoursesQueryHandler sut,
        GetProviderAvailableCoursesQuery request)
    {
        // Arrange
        var standards = new GetAllStandardsResponse()
        {
            Standards = new List<StandardModel>()
            {
                new StandardModel
                {
                    LarsCode = "TestLars1",
                    Title = "TestTitle1",
                    Level = 1,
                },
                new StandardModel
                {
                    LarsCode = "TestLars2",
                    Title = "TestTitle2",
                    Level = 2,
                }
            }
        };

        var providerAllowedCourses = new GetProviderAllowedCoursesResponse(
            new List<ProviderAllowedCourseModel>
            {
                new(
                    LarsCode: "TestLars1",
                    Title: "TestTitle1",
                    Level: 1,
                    LastDateStarts: null,
                    IsStartRestricted: false,
                    IsActive: false
                )
            });

        apiClientMock
            .Setup(a => a.GetWithResponseCode<GetAllStandardsResponse>(It.Is<GetAllStandardsRequest>(x => x.CourseType == request.CourseType)))
            .ReturnsAsync(new ApiResponse<GetAllStandardsResponse>(standards, HttpStatusCode.OK, ""));

        apiClientMock
            .Setup(a => a.GetWithResponseCode<GetProviderAllowedCoursesResponse>(It.Is<GetProviderAllowedCoursesRequest>(x => x.Ukprn == request.Ukprn && x.CourseType == request.CourseType)))
            .ReturnsAsync(new ApiResponse<GetProviderAllowedCoursesResponse>(providerAllowedCourses, HttpStatusCode.OK, ""));

        // Act
        var result = sut.Handle(request, CancellationToken.None);

        // Assert
        result.Result.Courses.Should().Contain(x => x.LarsCode == "TestLars2");
        result.Result.Courses.Should().NotContain(x => x.LarsCode == "TestLars1");
    }

    [Test, MoqAutoData]
    public async Task WhenStandardsReturnsCoursesAndProviderAllowedCoursesReturnsEmpty_ThenReturnsAllCoursesFromStandards(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] GetProviderAvailableCoursesQueryHandler sut,
        GetProviderAvailableCoursesQuery request)
    {
        // Arrange
        var standards = new GetAllStandardsResponse()
        {
            Standards = new List<StandardModel>()
            {
                new StandardModel
                {
                    LarsCode = "TestLars1",
                    Title = "TestTitle1",
                    Level = 1,
                },
                new StandardModel
                {
                    LarsCode = "TestLars2",
                    Title = "TestTitle2",
                    Level = 2,
                }
            }
        };

        var providerAllowedCourses = new GetProviderAllowedCoursesResponse(Enumerable.Empty<ProviderAllowedCourseModel>());

        apiClientMock
            .Setup(a => a.GetWithResponseCode<GetAllStandardsResponse>(It.Is<GetAllStandardsRequest>(x => x.CourseType == request.CourseType)))
            .ReturnsAsync(new ApiResponse<GetAllStandardsResponse>(standards, HttpStatusCode.OK, ""));

        apiClientMock
            .Setup(a => a.GetWithResponseCode<GetProviderAllowedCoursesResponse>(It.Is<GetProviderAllowedCoursesRequest>(x => x.Ukprn == request.Ukprn && x.CourseType == request.CourseType)))
            .ReturnsAsync(new ApiResponse<GetProviderAllowedCoursesResponse>(providerAllowedCourses, HttpStatusCode.OK, ""));

        // Act
        var result = sut.Handle(request, CancellationToken.None);

        // Assert
        result.Result.Courses.Should().Contain(x => x.LarsCode == "TestLars2");
        result.Result.Courses.Should().Contain(x => x.LarsCode == "TestLars1");
    }

    [Test, MoqAutoData]
    public async Task WhenStandardsAndProviderAllowedCoursesReturnsCourses_ThenVerifyApisAreInvoked(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] GetProviderAvailableCoursesQueryHandler sut,
        GetProviderAvailableCoursesQuery request,
        GetAllStandardsResponse standards,
        GetProviderAllowedCoursesResponse providerAllowedCourses)
    {
        // Arrange
        apiClientMock
            .Setup(a => a.GetWithResponseCode<GetAllStandardsResponse>(It.Is<GetAllStandardsRequest>(x => x.CourseType == request.CourseType)))
            .ReturnsAsync(new ApiResponse<GetAllStandardsResponse>(standards, HttpStatusCode.OK, ""));

        apiClientMock
            .Setup(a => a.GetWithResponseCode<GetProviderAllowedCoursesResponse>(It.Is<GetProviderAllowedCoursesRequest>(x => x.Ukprn == request.Ukprn && x.CourseType == request.CourseType)))
            .ReturnsAsync(new ApiResponse<GetProviderAllowedCoursesResponse>(providerAllowedCourses, HttpStatusCode.OK, ""));

        // Act
        await sut.Handle(request, CancellationToken.None);

        // Assert
        apiClientMock
            .Verify(a => a.GetWithResponseCode<GetAllStandardsResponse>(It.Is<GetAllStandardsRequest>(x => x.CourseType == request.CourseType)), Times.Once());
        apiClientMock
            .Verify(a => a.GetWithResponseCode<GetProviderAllowedCoursesResponse>(It.Is<GetProviderAllowedCoursesRequest>(x => x.Ukprn == request.Ukprn && x.CourseType == request.CourseType)), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task WhenGetStandardsReturnsUnsuccessfulResponse_ThenThrowsException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] GetProviderAvailableCoursesQueryHandler sut,
        GetProviderAvailableCoursesQuery request,
        GetAllStandardsResponse standards,
        GetProviderAllowedCoursesResponse providerAllowedCourses)
    {
        // Arrange
        apiClientMock
            .Setup(a => a.GetWithResponseCode<GetAllStandardsResponse>(It.Is<GetAllStandardsRequest>(x => x.CourseType == request.CourseType)))
            .ReturnsAsync(new ApiResponse<GetAllStandardsResponse>(standards, HttpStatusCode.InternalServerError, ""));

        apiClientMock
            .Setup(a => a.GetWithResponseCode<GetProviderAllowedCoursesResponse>(It.Is<GetProviderAllowedCoursesRequest>(x => x.Ukprn == request.Ukprn && x.CourseType == request.CourseType)))
            .ReturnsAsync(new ApiResponse<GetProviderAllowedCoursesResponse>(providerAllowedCourses, HttpStatusCode.OK, ""));

        // Act
        Func<Task> result = () => sut.Handle(request, CancellationToken.None);

        // Assert
        await result.Should().ThrowAsync<ApiResponseException>();
    }

    [Test, MoqAutoData]
    public async Task WhenGetProviderAllowedCoursesReturnsUnsuccessfulResponse_ThenThrowsException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] GetProviderAvailableCoursesQueryHandler sut,
        GetProviderAvailableCoursesQuery request,
        GetAllStandardsResponse standards,
        GetProviderAllowedCoursesResponse providerAllowedCourses)
    {
        // Arrange
        apiClientMock
            .Setup(a => a.GetWithResponseCode<GetAllStandardsResponse>(It.Is<GetAllStandardsRequest>(x => x.CourseType == request.CourseType)))
            .ReturnsAsync(new ApiResponse<GetAllStandardsResponse>(standards, HttpStatusCode.OK, ""));

        apiClientMock
            .Setup(a => a.GetWithResponseCode<GetProviderAllowedCoursesResponse>(It.Is<GetProviderAllowedCoursesRequest>(x => x.Ukprn == request.Ukprn && x.CourseType == request.CourseType)))
            .ReturnsAsync(new ApiResponse<GetProviderAllowedCoursesResponse>(providerAllowedCourses, HttpStatusCode.InternalServerError, ""));

        // Act
        Func<Task> result = () => sut.Handle(request, CancellationToken.None);

        // Assert
        await result.Should().ThrowAsync<ApiResponseException>();
    }
}
