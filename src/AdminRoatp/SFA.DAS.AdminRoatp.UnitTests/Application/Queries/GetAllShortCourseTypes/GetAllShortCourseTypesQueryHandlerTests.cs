using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetAllShortCourseTypes;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetAllShortCourseTypes;
public class GetAllShortCourseTypesQueryHandlerTests
{
    [Test, MoqAutoData]

    public async Task Handle_CourseTypeFound_ReturnsShortCourseTypesOnly(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClient,
        GetAllShortCourseTypesQueryHandler sut,
        GetAllShortCourseTypesQuery query)
    {
        // Arrange
        var apiResponse = new GetAllCourseTypesResponse()
        {
            CourseTypes = new List<CourseTypeSummary>
            {
                new CourseTypeSummary (1,"Standard Course",LearningType.Standard),
                new CourseTypeSummary (2,"Short Course",LearningType.ShortCourse)
            }
        };

        apiClient.Setup(a => a.GetWithResponseCode<GetAllCourseTypesResponse>(It.Is<GetAllCourseTypesRequest>(r => r.GetUrl.Equals(new GetAllCourseTypesRequest().GetUrl)))).ReturnsAsync(new ApiResponse<GetAllCourseTypesResponse>(apiResponse, HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result.CourseTypes.Count(), Is.EqualTo(1));
        result.CourseTypes.Should().OnlyContain(ct => ct.LearningType == LearningType.ShortCourse);
        apiClient.Verify(x => x.GetWithResponseCode<GetAllCourseTypesResponse>(It.Is<GetAllCourseTypesRequest>(r => r.GetUrl.Equals(new GetAllCourseTypesRequest().GetUrl))), Times.Once);
    }

    [Test, MoqAutoData]

    public async Task Handle_CourseTypeNotFound_ReturnsEmpty(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClient,
        GetAllShortCourseTypesQueryHandler sut,
        GetAllShortCourseTypesQuery query)
    {
        // Arrange
        var apiResponse = new GetAllCourseTypesResponse();

        apiClient.Setup(a => a.GetWithResponseCode<GetAllCourseTypesResponse>(It.Is<GetAllCourseTypesRequest>(r => r.GetUrl.Equals(new GetAllCourseTypesRequest().GetUrl)))).ReturnsAsync(new ApiResponse<GetAllCourseTypesResponse>(apiResponse, HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.CourseTypes.Should().BeEmpty();
    }
}