using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Commands.PatchProviderAllowedCourse;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Commands.PatchProviderAllowedCourse;

public class PatchProviderAllowedCourseCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task WhenHandlingRequest_ThenVerifyApiClientCalled(
    [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
    [Greedy] PatchProviderAllowedCourseCommandHandler sut,
    PatchProviderAllowedCourseCommand command)
    {
        // Arrange
        apiClientMock
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchProviderAllowedCourseRequest>()))
            .ReturnsAsync(new ApiResponse<string>(string.Empty, HttpStatusCode.OK, string.Empty));

        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert
        apiClientMock.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchProviderAllowedCourseRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task WhenApiErrorIsReturned_ThenShouldThrowApiResponseException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] PatchProviderAllowedCourseCommandHandler sut,
        PatchProviderAllowedCourseCommand command)
    {
        // Arrange
        apiClientMock
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchProviderAllowedCourseRequest>()))
            .ReturnsAsync(new ApiResponse<string>(string.Empty, HttpStatusCode.BadRequest, string.Empty));

        // Act
        Func<Task> action = () => sut.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ApiResponseException>();
    }
}
