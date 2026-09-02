using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using SFA.DAS.AdminRoatp.Application.Commands.UpsertProviderAllowedCourse;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Commands.UpsertProviderAllowedCourse;

public class UpsertProviderAllowedCourseCommandHandlerTests
{

    [Test, MoqAutoData]
    public async Task WhenHandlingRequest_ThenVerifyUpsertProviderAllowedCoursePostApiIsCalled(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpServiceApiClientMock,
        [Greedy] UpsertProviderAllowedCourseCommandHandler sut,
        UpsertProviderAllowedCourseCommand command,
        StandardModel standard)
    {
        // Arrange
        apiClientMock
            .Setup(x => x.PostWithResponseCode<Unit>(It.IsAny<UpsertProviderAllowedCourseRequest>()))
            .ReturnsAsync(new ApiResponse<Unit>(Unit.Value, HttpStatusCode.OK, string.Empty));

        apiClientMock
            .Setup(x => x.GetWithResponseCode<StandardModel>(It.Is<GetStandardByLarsCodeRequest>(r => r.LarsCode == command.LarsCode)))
            .ReturnsAsync(new ApiResponse<StandardModel>(standard, HttpStatusCode.OK, string.Empty));

        roatpServiceApiClientMock
            .Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<UpdateCourseTypesRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.OK, string.Empty));

        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert
        apiClientMock.Verify(x => x.PostWithResponseCode<Unit>(
            It.Is<UpsertProviderAllowedCourseRequest>(r =>
                r.Ukprn == command.Ukprn &&
                r.LarsCode == command.LarsCode &&
                ((UpsertProviderAllowedCourseModel)r.Data).UserId == command.UserId &&
                ((UpsertProviderAllowedCourseModel)r.Data).UserDisplayName == command.UserDisplayName &&
                ((UpsertProviderAllowedCourseModel)r.Data).LastDateStarts == command.LastDateStarts)),
            Times.Once);
    }

    [Test, MoqAutoData]
    public async Task WhenHandlingRequest_ThenVerifyGetStadardApiIsCalled(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpServiceApiClientMock,
        [Greedy] UpsertProviderAllowedCourseCommandHandler sut,
        UpsertProviderAllowedCourseCommand command,
        StandardModel standard)
    {
        // Arrange
        apiClientMock
            .Setup(x => x.PostWithResponseCode<Unit>(It.IsAny<UpsertProviderAllowedCourseRequest>()))
            .ReturnsAsync(new ApiResponse<Unit>(Unit.Value, HttpStatusCode.OK, string.Empty));

        apiClientMock
            .Setup(x => x.GetWithResponseCode<StandardModel>(It.Is<GetStandardByLarsCodeRequest>(r => r.LarsCode == command.LarsCode)))
            .ReturnsAsync(new ApiResponse<StandardModel>(standard, HttpStatusCode.OK, string.Empty));

        roatpServiceApiClientMock
            .Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<UpdateCourseTypesRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.OK, string.Empty));

        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert
        apiClientMock
            .Verify(x => x.GetWithResponseCode<StandardModel>(It.Is<GetStandardByLarsCodeRequest>(r => r.LarsCode == command.LarsCode)),
            Times.Once());
    }

    [Test, MoqAutoData]
    public async Task WhenHandlingRequest_AndCourseTypeIsApprenticeship_ThenVerifyUpdateCourseTypesApiIsNotCalled(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpServiceApiClientMock,
        [Greedy] UpsertProviderAllowedCourseCommandHandler sut,
        UpsertProviderAllowedCourseCommand command,
        StandardModel standard)
    {
        // Arrange
        standard.CourseType = CourseType.Apprenticeship;

        apiClientMock
            .Setup(x => x.PostWithResponseCode<Unit>(It.IsAny<UpsertProviderAllowedCourseRequest>()))
            .ReturnsAsync(new ApiResponse<Unit>(Unit.Value, HttpStatusCode.OK, string.Empty));

        apiClientMock
            .Setup(x => x.GetWithResponseCode<StandardModel>(It.Is<GetStandardByLarsCodeRequest>(r => r.LarsCode == command.LarsCode)))
            .ReturnsAsync(new ApiResponse<StandardModel>(standard, HttpStatusCode.OK, string.Empty));

        roatpServiceApiClientMock
            .Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<UpdateCourseTypesRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.OK, string.Empty));

        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert
        roatpServiceApiClientMock
            .Verify(x => x.PutWithResponseCode<NullResponse>(It.IsAny<UpdateCourseTypesRequest>()),
            Times.Never());
    }

    [Test, MoqAutoData]
    public async Task WhenHandlingRequest_AndCourseTypeIsShortCourse_ThenVerifyUpdateCourseTypesApiIsCalled(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpServiceApiClientMock,
        [Greedy] UpsertProviderAllowedCourseCommandHandler sut,
        UpsertProviderAllowedCourseCommand command,
        StandardModel standard)
    {
        // Arrange
        standard.CourseType = CourseType.ShortCourse;

        apiClientMock
            .Setup(x => x.PostWithResponseCode<Unit>(It.IsAny<UpsertProviderAllowedCourseRequest>()))
            .ReturnsAsync(new ApiResponse<Unit>(Unit.Value, HttpStatusCode.OK, string.Empty));

        apiClientMock
            .Setup(x => x.GetWithResponseCode<StandardModel>(It.Is<GetStandardByLarsCodeRequest>(r => r.LarsCode == command.LarsCode)))
            .ReturnsAsync(new ApiResponse<StandardModel>(standard, HttpStatusCode.OK, string.Empty));

        roatpServiceApiClientMock
            .Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<UpdateCourseTypesRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.OK, string.Empty));

        // Act
        await sut.Handle(command, CancellationToken.None);

        // Assert
        roatpServiceApiClientMock.Verify(
        x => x.PutWithResponseCode<NullResponse>(It.Is<UpdateCourseTypesRequest>(r =>
                r.ukprn == command.Ukprn &&
                ((UpdateCourseTypesModel)r.Data).UserId == command.UserId &&
                ((UpdateCourseTypesModel)r.Data).CourseTypeIds.SequenceEqual(new[] { (int)CourseType.ShortCourse })
            )),
        Times.Once);
    }

    [Test, MoqAutoData]
    public async Task WhenApiErrorIsReturnedFromUpsertProviderAllowedCourse_ThenShouldThrowApiResponseException(
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

    [Test, MoqAutoData]
    public async Task WhenApiErrorIsReturnedFromGetStandard_ThenShouldThrowApiResponseException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Greedy] UpsertProviderAllowedCourseCommandHandler sut,
        UpsertProviderAllowedCourseCommand command)
    {
        // Arrange
        apiClientMock
            .Setup(x => x.PostWithResponseCode<Unit>(It.IsAny<UpsertProviderAllowedCourseRequest>()))
            .ReturnsAsync(new ApiResponse<Unit>(Unit.Value, HttpStatusCode.OK, string.Empty));

        apiClientMock
            .Setup(x => x.GetWithResponseCode<StandardModel>(It.Is<GetStandardByLarsCodeRequest>(r => r.LarsCode == command.LarsCode)))
            .ReturnsAsync(new ApiResponse<StandardModel>(new StandardModel(), HttpStatusCode.BadRequest, string.Empty));

        // Act
        Func<Task> action = () => sut.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ApiResponseException>();
    }

    [Test, MoqAutoData]
    public async Task WhenApiErrorIsReturnedFromUpdateCourseTypes_ThenShouldThrowApiResponseException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpServiceApiClientMock,
        [Greedy] UpsertProviderAllowedCourseCommandHandler sut,
        UpsertProviderAllowedCourseCommand command,
        StandardModel standard)
    {
        // Arrange
        standard.CourseType = CourseType.ShortCourse;

        apiClientMock
            .Setup(x => x.PostWithResponseCode<Unit>(It.IsAny<UpsertProviderAllowedCourseRequest>()))
            .ReturnsAsync(new ApiResponse<Unit>(Unit.Value, HttpStatusCode.OK, string.Empty));

        apiClientMock
            .Setup(x => x.GetWithResponseCode<StandardModel>(It.Is<GetStandardByLarsCodeRequest>(r => r.LarsCode == command.LarsCode)))
            .ReturnsAsync(new ApiResponse<StandardModel>(standard, HttpStatusCode.OK, string.Empty));

        roatpServiceApiClientMock
            .Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<UpdateCourseTypesRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.BadRequest, string.Empty));

        // Act
        Func<Task> action = () => sut.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ApiResponseException>();
    }
}
