using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetAllCourses;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetAllCourses;

public class GetAllCoursesQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task WhenHandlingRequest_ThenReturnsCoursesFromStandards(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetAllCoursesQueryHandler sut)
    {
        var standards = new GetAllStandardsResponse
        {
            Standards =
            [
                new StandardModel
                {
                    LarsCode = "123",
                    CourseType = CourseType.ShortCourse,
                    LearningType = Common.Domain.Types.LearningType.FoundationApprenticeship,
                    Title = "Example course",
                    Level = 2,
                    ApprovalBody = "Not returned"
                }
            ]
        };

        apiClientMock
            .Setup(c => c.GetWithResponseCode<GetAllStandardsResponse>(It.Is<GetStandardsRequest>(r => r.GetUrl == "standards")))
            .ReturnsAsync(new ApiResponse<GetAllStandardsResponse>(standards, HttpStatusCode.OK, ""));

        var result = await sut.Handle(new GetAllCoursesQuery(), CancellationToken.None);

        using (new AssertionScope())
        {
            result.Courses.Should().BeEquivalentTo(
            [
                new CourseModel
                {
                    LarsCode = "123",
                    CourseType = CourseType.ShortCourse,
                    LearningType = Common.Domain.Types.LearningType.FoundationApprenticeship,
                    Title = "Example course",
                    Level = 2
                }
            ]);
            apiClientMock.Verify(
                c => c.GetWithResponseCode<GetAllStandardsResponse>(It.Is<GetStandardsRequest>(r => r.GetUrl == "standards")),
                Times.Once);
        }
    }

    [Test, MoqAutoData]
    public async Task WhenHandlingRequestAndStandardsAreEmpty_ThenReturnsEmptyCourses(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetAllCoursesQueryHandler sut)
    {
        apiClientMock
            .Setup(c => c.GetWithResponseCode<GetAllStandardsResponse>(It.IsAny<GetStandardsRequest>()))
            .ReturnsAsync(new ApiResponse<GetAllStandardsResponse>(new GetAllStandardsResponse(), HttpStatusCode.OK, ""));

        var result = await sut.Handle(new GetAllCoursesQuery(), CancellationToken.None);

        result.Courses.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task WhenHandlingRequestAndApiReturnsInvalidResponseCode_ThenThrowsApiResponseException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetAllCoursesQueryHandler sut)
    {
        apiClientMock
            .Setup(c => c.GetWithResponseCode<GetAllStandardsResponse>(It.IsAny<GetStandardsRequest>()))
            .ReturnsAsync(new ApiResponse<GetAllStandardsResponse>(new GetAllStandardsResponse(), HttpStatusCode.InternalServerError, ""));

        Func<Task> act = () => sut.Handle(new GetAllCoursesQuery(), CancellationToken.None);

        await act.Should().ThrowAsync<ApiResponseException>();
    }
}
