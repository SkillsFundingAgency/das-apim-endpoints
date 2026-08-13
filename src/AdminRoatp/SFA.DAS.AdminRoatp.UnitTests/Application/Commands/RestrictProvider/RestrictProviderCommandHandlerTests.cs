using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using SFA.DAS.AdminRoatp.Application.Commands.RestrictProvider;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Commands.RestrictProvider;

public class RestrictProviderCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task WhenHandlingRequest_ThenVerifyApiClientCalled(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] RestrictProviderCommandHandler sut,
        RestrictProviderCommand command)
    {
        // Arrange
        apiClientMock
            .Setup(x => x.PostWithResponseCode<Unit>(
                It.IsAny<RestrictProviderRequest>()))
            .ReturnsAsync(new ApiResponse<Unit>(
                Unit.Value,
                HttpStatusCode.NoContent,
                string.Empty));

        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert
        apiClientMock.Verify(x => x.PostWithResponseCode<Unit>(
            It.Is<RestrictProviderRequest>(r =>
                r.Ukprn == command.Ukprn &&
                r.CourseType == command.CourseType &&
                ((RestrictProviderModel)r.Data).UserId == command.UserId &&
                ((RestrictProviderModel)r.Data).UserDisplayName == command.UserDisplayName)),
            Times.Once);
    }

    [Test, MoqAutoData]
    public async Task WhenApiErrorIsReturned_ThenShouldThrowApiResponseException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] RestrictProviderCommandHandler sut,
        RestrictProviderCommand command)
    {
        // Arrange
        apiClientMock
            .Setup(x => x.PostWithResponseCode<Unit>(
                It.IsAny<RestrictProviderRequest>()))
            .ReturnsAsync(new ApiResponse<Unit>(
                Unit.Value,
                HttpStatusCode.BadRequest,
                string.Empty));

        // Act
        Func<Task> action = () => sut.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ApiResponseException>();
    }
}
