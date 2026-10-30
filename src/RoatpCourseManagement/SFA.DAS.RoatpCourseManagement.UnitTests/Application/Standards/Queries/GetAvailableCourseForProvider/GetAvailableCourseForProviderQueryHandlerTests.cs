using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCourseForProvider;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Queries.GetAvailableCourseForProvider;

public class GetAvailableCourseForProviderQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task WhenApiReturnsCourse_ThenReturnsResponseBody(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] GetAvailableCourseForProviderQueryHandler sut,
        GetAvailableCourseForProviderQuery request,
        GetAvailableCourseForProviderQueryResult response)
    {
        // Arrange
        apiClientMock
            .Setup(client => client.GetWithResponseCode<GetAvailableCourseForProviderQueryResult>(It.IsAny<GetAvailableCourseForProviderRequest>()))
            .ReturnsAsync(new ApiResponse<GetAvailableCourseForProviderQueryResult>(response, HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(response);
    }

    [Test, MoqAutoData]
    public async Task WhenApiReturnsNotFound_ThenReturnsNull(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] GetAvailableCourseForProviderQueryHandler sut,
        GetAvailableCourseForProviderQuery request,
        GetAvailableCourseForProviderQueryResult response)
    {
        // Arrange
        apiClientMock
            .Setup(client => client.GetWithResponseCode<GetAvailableCourseForProviderQueryResult>(It.IsAny<GetAvailableCourseForProviderRequest>()))
            .ReturnsAsync(new ApiResponse<GetAvailableCourseForProviderQueryResult>(response, HttpStatusCode.NotFound, ""));

        // Act
        var result = await sut.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task WhenApiReturnsUnsuccessfulResponse_ThenThrowsApiResponseException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] GetAvailableCourseForProviderQueryHandler sut,
        GetAvailableCourseForProviderQuery request,
        GetAvailableCourseForProviderQueryResult response)
    {
        // Arrange
        apiClientMock
            .Setup(client => client.GetWithResponseCode<GetAvailableCourseForProviderQueryResult>(It.IsAny<GetAvailableCourseForProviderRequest>()))
            .ReturnsAsync(new ApiResponse<GetAvailableCourseForProviderQueryResult>(response, HttpStatusCode.InternalServerError, ""));

        // Act
        var act = () => sut.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ApiResponseException>();
    }
}
