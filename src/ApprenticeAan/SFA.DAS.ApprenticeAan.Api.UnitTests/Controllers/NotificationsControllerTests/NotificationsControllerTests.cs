using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestEase;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Api.Models.Notifications;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Notifications;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers.NotificationsControllerTests;
public class NotificationsControllerTests
{
    [Test, MoqAutoData]
    public async Task GetNotification_InvokesApiClientAndNullReturned_ReturnsOkWithNullValueResponse(
    [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
    [Greedy] NotificationsController sut,
    GetNotificationResponse notificationsResponse,
    Guid notificationId,
    Guid requestedByMemberId)
    {
        //Arrange
        notificationsResponse.MemberId = requestedByMemberId;
        apiClientMock.Setup(a => a.GetNotification(notificationId, requestedByMemberId, It.IsAny<CancellationToken>())).ReturnsAsync(new Response<GetNotificationResponse?>(string.Empty, new(HttpStatusCode.NotFound), () => null));

        //Act
        var actual = await sut.GetNotification(notificationId, requestedByMemberId, It.IsAny<CancellationToken>());
        var actualObjectResult = actual as ObjectResult;

        //Assert
        using (new AssertionScope())
        {
            apiClientMock.Verify(a => a.GetNotification(notificationId, requestedByMemberId, It.IsAny<CancellationToken>()));
            actualObjectResult.Should().BeOfType<OkObjectResult>();
            actualObjectResult!.StatusCode.Should().Be(200);
            actualObjectResult.Value.Should().BeNull();
        }
    }

    [Test, MoqAutoData]
    public async Task GetNotification_InvokesApiClientAndNotificationReturned_ReturnsOkWithValueResponse(
    [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
    [Greedy] NotificationsController sut,
    GetNotificationResponse notificationsResponse,
    Guid notificationId,
    Guid requestedByMemberId)
    {
        //Arrange
        notificationsResponse.MemberId = requestedByMemberId;
        apiClientMock.Setup(a => a.GetNotification(notificationId, requestedByMemberId, It.IsAny<CancellationToken>())).ReturnsAsync(new Response<GetNotificationResponse?>(string.Empty, new(HttpStatusCode.OK), () => notificationsResponse));

        //Act
        var actual = await sut.GetNotification(notificationId, requestedByMemberId, It.IsAny<CancellationToken>());
        var actualObjectResult = actual as ObjectResult;

        //Assert
        using (new AssertionScope())
        {
            apiClientMock.Verify(a => a.GetNotification(notificationId, requestedByMemberId, It.IsAny<CancellationToken>()));
            actualObjectResult.Should().BeOfType<OkObjectResult>();
            actualObjectResult!.Value.Should().Be(notificationsResponse);
            actualObjectResult.StatusCode.Should().Be(200);
        }
    }

    [Test, MoqAutoData]
    public async Task GetNotification_InvokesApiClientAndUnexpectedResponseReturned_ReturnsInvalidOperationException(
    [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
    [Greedy] NotificationsController sut,
    GetNotificationResponse notificationsResponse,
    Guid notificationId,
    Guid requestedByMemberId)
    {
        //Arrange
        notificationsResponse.MemberId = requestedByMemberId;
        apiClientMock.Setup(a => a.GetNotification(notificationId, requestedByMemberId, It.IsAny<CancellationToken>())).ReturnsAsync(new Response<GetNotificationResponse?>(string.Empty, new(HttpStatusCode.Unauthorized), () => null));

        //Act
        Func<Task> actual = async () => await sut.GetNotification(notificationId, requestedByMemberId, It.IsAny<CancellationToken>());

        //Assert
        using (new AssertionScope())
        {
            await actual.Should().ThrowAsync<InvalidOperationException>().WithMessage("Get notification didn't come back with a successful response");
            apiClientMock.Verify(a => a.GetNotification(notificationId, requestedByMemberId, It.IsAny<CancellationToken>()));
        }
    }

    [Test, MoqAutoData]
    public async Task CreateNotification_InvokesCommand(
    [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
    [Greedy] NotificationsController sut,
    CreateNotificationModel model,
    Guid requestedByMemberId,
    CancellationToken cancellationToken)
    {
        //Arrange
        Response<GetNotificationResponse> notificationsResponse = new Response<GetNotificationResponse>(string.Empty, new(HttpStatusCode.Created), () => null!);
        apiClientMock.Setup(a => a.PostNotification(requestedByMemberId, It.IsAny<PostNotificationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(notificationsResponse);

        //Act
        var response = await sut.CreateNotification(requestedByMemberId, model, cancellationToken);

        //Assert
        apiClientMock.Verify(a => a.PostNotification(requestedByMemberId, It.IsAny<PostNotificationRequest>(), It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public void CreateNotification_InvalidCommand_ThrowsInvalidOperationException(
    [Frozen] Mock<IMediator> mediatorMock,
    [Greedy] NotificationsController sut,
    CreateNotificationModel model,
    Guid requestedByMemberId,
    Response<GetNotificationResponse> expected,
    CancellationToken cancellationToken)
    {
        //Arrange
        expected.ResponseMessage.StatusCode = HttpStatusCode.NotFound;
        mediatorMock.Setup(m => m.Send(It.IsAny<PostNotificationRequest>(), cancellationToken)).ReturnsAsync(expected.GetContent());

        //Act
        Assert.That(() => sut.CreateNotification(requestedByMemberId, model, cancellationToken), Throws.InvalidOperationException);
    }

    [Test, MoqAutoData]
    public async Task CreateNotification_ValidCommand_ReturnsOk(
    [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
    [Greedy] NotificationsController sut,
    CreateNotificationModel model,
    Guid requestedByMemberId,
    CancellationToken cancellationToken)
    {
        //Arrange
        Response<GetNotificationResponse> notificationsResponse = new Response<GetNotificationResponse>(string.Empty, new(HttpStatusCode.Created), () => null!);
        apiClientMock.Setup(a => a.PostNotification(requestedByMemberId, It.IsAny<PostNotificationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(notificationsResponse);

        //Act
        var response = await sut.CreateNotification(requestedByMemberId, model, cancellationToken);
        var actualObjectResult = response as ObjectResult;

        //Assert
        using (new AssertionScope())
        {
            apiClientMock.Verify(a => a.PostNotification(requestedByMemberId, It.IsAny<PostNotificationRequest>(), It.IsAny<CancellationToken>()));
            actualObjectResult.Should().BeOfType<OkObjectResult>();
        }
    }
}
