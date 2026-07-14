using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using SFA.DAS.AdminRoatp.Application.Commands.UpsertProviderAllowedCourse;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Commands.UpsertProviderAllowedCourse;

public class UpsertProviderAllowedCourseCommandHandlerTests
{

    [Test, MoqAutoData]
    public async Task WhenHandlingRequest_ThenVerifyApiClientCalled(
    [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
    [Greedy] UpsertProviderAllowedCourseCommandHandler sut,
    UpsertProviderAllowedCourseCommand command)
    {
        // Arrange
        apiClientMock
            .Setup(x => x.PostWithResponseCode<Unit>(It.IsAny<UpsertProviderAllowedCourseRequest>()))
            .ReturnsAsync(new ApiResponse<Unit>(Unit.Value, HttpStatusCode.OK, string.Empty));

        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert
        apiClientMock.Verify(x => x.PostWithResponseCode<Unit>(
            It.Is<UpsertProviderAllowedCourseRequest>(x => x.Data == command)),
            Times.Once);
    }

    [Test, MoqAutoData]
    public async Task WhenApiErrorIsReturned_ThenShouldThrowApiResponseException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] UpsertProviderAllowedCourseCommandHandler sut,
        UpsertProviderAllowedCourseCommand command)
    {
        // Arrange
        apiClientMock
            .Setup(x => x.PostWithResponseCode<Unit>(It.IsAny<UpsertProviderAllowedCourseRequest>()))
            .ReturnsAsync(new ApiResponse<Unit>(Unit.Value, HttpStatusCode.BadRequest, string.Empty));

        // Act
        Func<Task> action = () => sut.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ApiResponseException>();
    }
}
