using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetProvidersAllowedToDeliverCourse;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetProvidersAllowedToDeliverCourse;

public class GetProvidersAllowedToDeliverCourseQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task WhenHandlingRequest_ThenReturnsResults(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            RestrictedCourseDetailsModel response,
            GetProvidersAllowedToDeliverCourseQuery query,
            GetProvidersAllowedToDeliverCourseQueryHandler sut)
    {
        // Arrange
        apiClientMock.Setup(c => c.GetWithResponseCode<RestrictedCourseDetailsModel>(It.IsAny<GetAllowedProvidersRequest>())).ReturnsAsync(new ApiResponse<RestrictedCourseDetailsModel>(response, HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, new CancellationToken());

        // Assert
        result.Should().BeEquivalentTo(response);
    }

    [Test, MoqAutoData]
    public void WhenHandlingRequestAndApiReturnsInvalidResponseCode_ThenThrowsApiResponseException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetProvidersAllowedToDeliverCourseQuery query,
        GetProvidersAllowedToDeliverCourseQueryHandler sut)
    {
        // Arrange
        apiClientMock.Setup(c => c.GetWithResponseCode<RestrictedCourseDetailsModel>(It.IsAny<GetAllowedProvidersRequest>())).ReturnsAsync(new ApiResponse<RestrictedCourseDetailsModel>(new RestrictedCourseDetailsModel(), HttpStatusCode.InternalServerError, ""));

        // Act & Assert
        Assert.ThrowsAsync<ApiResponseException>(() => sut.Handle(query, new CancellationToken()));
    }
}
