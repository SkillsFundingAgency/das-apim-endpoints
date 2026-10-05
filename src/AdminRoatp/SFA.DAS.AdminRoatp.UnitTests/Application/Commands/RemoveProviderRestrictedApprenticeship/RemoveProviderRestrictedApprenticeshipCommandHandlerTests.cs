using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using SFA.DAS.AdminRoatp.Application.Commands.RemoveProviderRestrictedApprenticeship;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Commands.RemoveProviderRestrictedApprenticeship;

public class RemoveProviderRestrictedApprenticeshipCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task WhenDoesNotExistInProviderAllowedCourse_ThenAddsEntryInProviderAllowedCourse(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] RemoveProviderRestrictedApprenticeshipCommandHandler sut,
        RemoveProviderRestrictedApprenticeshipCommand command)
    {
        //Arrange
        apiClientMock
            .Setup(x => x.GetWithResponseCode<GetProviderAllowedCourseDetailsResponse>(It.IsAny<GetProviderAllowedCourseDetailsRequest>()))
            .ReturnsAsync(new ApiResponse<GetProviderAllowedCourseDetailsResponse>(new GetProviderAllowedCourseDetailsResponse(), HttpStatusCode.NoContent, ""));

        apiClientMock
            .Setup(x => x.PostWithResponseCode<Unit>(It.IsAny<AddProviderAllowedCourseRequest>()))
            .ReturnsAsync(new ApiResponse<Unit>(Unit.Value, HttpStatusCode.OK, string.Empty));

        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert
        apiClientMock.Verify(x => x.PostWithResponseCode<Unit>(
            It.Is<AddProviderAllowedCourseRequest>(r =>
                r.Ukprn == command.Ukprn &&
                r.LarsCode == command.LarsCode &&
                ((AddProviderAllowedCourseModel)r.Data).UserId == command.UserId &&
                ((AddProviderAllowedCourseModel)r.Data).UserDisplayName == command.UserDisplayName &&
                ((AddProviderAllowedCourseModel)r.Data).LastDateStarts == null)),
            Times.Once);
    }

    [Test, MoqAutoData]
    public async Task WhenExistInProviderAllowedCourse_AndCourseIsRestricted_ThenUpdatesLastDateStarts(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] RemoveProviderRestrictedApprenticeshipCommandHandler sut,
        RemoveProviderRestrictedApprenticeshipCommand command,
        GetProviderAllowedCourseDetailsResponse providerAllowedCourseDetailsResponse)
    {
        //Arrange
        providerAllowedCourseDetailsResponse.IsCourseRestricted = true;

        apiClientMock
            .Setup(x => x.GetWithResponseCode<GetProviderAllowedCourseDetailsResponse>(It.IsAny<GetProviderAllowedCourseDetailsRequest>()))
            .ReturnsAsync(new ApiResponse<GetProviderAllowedCourseDetailsResponse>(providerAllowedCourseDetailsResponse, HttpStatusCode.OK, ""));

        apiClientMock
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchProviderAllowedCourseRequest>()))
            .ReturnsAsync(new ApiResponse<string>(string.Empty, HttpStatusCode.OK, string.Empty));

        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert
        apiClientMock.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchProviderAllowedCourseRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task WhenExistInProviderAllowedCourse_AndCourseIsNotRestricted_ThenDeletesRecord(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] RemoveProviderRestrictedApprenticeshipCommandHandler sut,
        RemoveProviderRestrictedApprenticeshipCommand command,
        GetProviderAllowedCourseDetailsResponse providerAllowedCourseDetailsResponse)
    {
        //Arrange
        providerAllowedCourseDetailsResponse.IsCourseRestricted = false;

        apiClientMock
            .Setup(x => x.GetWithResponseCode<GetProviderAllowedCourseDetailsResponse>(It.IsAny<GetProviderAllowedCourseDetailsRequest>()))
            .ReturnsAsync(new ApiResponse<GetProviderAllowedCourseDetailsResponse>(providerAllowedCourseDetailsResponse, HttpStatusCode.OK, ""));

        apiClientMock
            .Setup(x => x.DeleteWithResponseCode<Unit>(It.IsAny<DeleteProviderAllowedCourseRequest>()))
            .ReturnsAsync(new ApiResponse<Unit>(Unit.Value, HttpStatusCode.OK, string.Empty));

        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert
        apiClientMock.Verify(x => x.DeleteWithResponseCode<Unit>(
            It.Is<DeleteProviderAllowedCourseRequest>(r =>
                r.Ukprn == command.Ukprn &&
                r.LarsCode == command.LarsCode &&
                r.UserId == command.UserId &&
                r.UserDisplayName == command.UserDisplayName)),
            Times.Once);
    }

    [Test, MoqAutoData]
    public async Task WhenGetProviderAllowedCourseDetailsReturnsError_ThenShouldThrowApiResponseException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] RemoveProviderRestrictedApprenticeshipCommandHandler sut,
        RemoveProviderRestrictedApprenticeshipCommand command)
    {
        //Arrange
        apiClientMock
            .Setup(x => x.GetWithResponseCode<GetProviderAllowedCourseDetailsResponse>(It.IsAny<GetProviderAllowedCourseDetailsRequest>()))
            .ReturnsAsync(new ApiResponse<GetProviderAllowedCourseDetailsResponse>(new GetProviderAllowedCourseDetailsResponse(), HttpStatusCode.InternalServerError, ""));

        // Act
        Func<Task> action = () => sut.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ApiResponseException>();
    }

    [Test, MoqAutoData]
    public async Task WhenAddProviderAllowedCourseReturnsError_ThenShouldThrowApiResponseException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] RemoveProviderRestrictedApprenticeshipCommandHandler sut,
        RemoveProviderRestrictedApprenticeshipCommand command)
    {
        //Arrange
        apiClientMock
            .Setup(x => x.GetWithResponseCode<GetProviderAllowedCourseDetailsResponse>(It.IsAny<GetProviderAllowedCourseDetailsRequest>()))
            .ReturnsAsync(new ApiResponse<GetProviderAllowedCourseDetailsResponse>(new GetProviderAllowedCourseDetailsResponse(), HttpStatusCode.NoContent, ""));

        apiClientMock
            .Setup(x => x.PostWithResponseCode<Unit>(It.IsAny<AddProviderAllowedCourseRequest>()))
            .ReturnsAsync(new ApiResponse<Unit>(Unit.Value, HttpStatusCode.InternalServerError, string.Empty));

        // Act
        Func<Task> action = () => sut.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ApiResponseException>();
    }

    [Test, MoqAutoData]
    public async Task WhenPatchProviderAllowedCourseReturnsError_ThenShouldThrowApiResponseException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] RemoveProviderRestrictedApprenticeshipCommandHandler sut,
        RemoveProviderRestrictedApprenticeshipCommand command,
        GetProviderAllowedCourseDetailsResponse providerAllowedCourseDetailsResponse)
    {
        //Arrange
        providerAllowedCourseDetailsResponse.IsCourseRestricted = true;

        apiClientMock
            .Setup(x => x.GetWithResponseCode<GetProviderAllowedCourseDetailsResponse>(It.IsAny<GetProviderAllowedCourseDetailsRequest>()))
            .ReturnsAsync(new ApiResponse<GetProviderAllowedCourseDetailsResponse>(providerAllowedCourseDetailsResponse, HttpStatusCode.OK, ""));

        apiClientMock
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchProviderAllowedCourseRequest>()))
            .ReturnsAsync(new ApiResponse<string>(string.Empty, HttpStatusCode.InternalServerError, string.Empty));

        // Act
        Func<Task> action = () => sut.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ApiResponseException>();
    }

    [Test, MoqAutoData]
    public async Task WhenDeleteProviderAllowedCourseReturnsError_ThenShouldThrowApiResponseException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] RemoveProviderRestrictedApprenticeshipCommandHandler sut,
        RemoveProviderRestrictedApprenticeshipCommand command,
        GetProviderAllowedCourseDetailsResponse providerAllowedCourseDetailsResponse)
    {
        //Arrange
        providerAllowedCourseDetailsResponse.IsCourseRestricted = false;

        apiClientMock
            .Setup(x => x.GetWithResponseCode<GetProviderAllowedCourseDetailsResponse>(It.IsAny<GetProviderAllowedCourseDetailsRequest>()))
            .ReturnsAsync(new ApiResponse<GetProviderAllowedCourseDetailsResponse>(providerAllowedCourseDetailsResponse, HttpStatusCode.OK, ""));

        apiClientMock
            .Setup(x => x.DeleteWithResponseCode<Unit>(It.IsAny<DeleteProviderAllowedCourseRequest>()))
            .ReturnsAsync(new ApiResponse<Unit>(Unit.Value, HttpStatusCode.InternalServerError, string.Empty));

        // Act
        Func<Task> action = () => sut.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ApiResponseException>();
    }
}
