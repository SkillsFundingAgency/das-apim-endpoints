using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestEase;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.InnerApi.Notifications;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.InnerApi.Notifications.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.NotificationsControllerTests;

public class CreateNotificationTests
{
    [Test, MoqAutoData]
    public async Task CreateNotification_InvokesCommand(
    [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
    [Greedy] NotificationsController sut,
    PostNotificationRequest postNotificationRequest,
    Guid requestedByMemberId,
    CancellationToken cancellationToken)
    {
        //Arrange
        Response<GetNotificationResponse> notificationsResponse = new Response<GetNotificationResponse>(string.Empty, new(HttpStatusCode.Created), () => null!);
        apiClientMock.Setup(a => a.PostNotification(requestedByMemberId, It.IsAny<PostNotificationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(notificationsResponse);

        //Act
        var response = await sut.CreateNotification(requestedByMemberId, postNotificationRequest, cancellationToken);

        //Assert
        apiClientMock.Verify(a => a.PostNotification(requestedByMemberId, It.IsAny<PostNotificationRequest>(), It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public void CreateNotification_HttpStatusCodeInternalServerError_ThrowsInvalidOperationException(
    [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
    [Greedy] NotificationsController sut,
    PostNotificationRequest postNotificationRequest,
    Guid requestedByMemberId,
    CancellationToken cancellationToken)
    {
        //Arrange
        Response<GetNotificationResponse> notificationsResponse = new Response<GetNotificationResponse>(string.Empty, new(HttpStatusCode.InternalServerError), () => null!);
        apiClientMock.Setup(a => a.PostNotification(requestedByMemberId, It.IsAny<PostNotificationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(notificationsResponse);

        //Act
        Assert.That(() => sut.CreateNotification(requestedByMemberId, postNotificationRequest, cancellationToken), Throws.InvalidOperationException);
    }

    [Test, MoqAutoData]
    public async Task CreateNotification_HttpStatusCodeNotFound_ReturnsNull(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        [Greedy] NotificationsController sut,
        PostNotificationRequest postNotificationRequest,
        Guid requestedByMemberId,
        CancellationToken cancellationToken)
    {
        //Arrange
        Response<GetNotificationResponse> notificationsResponse = new Response<GetNotificationResponse>(string.Empty, new(HttpStatusCode.NotFound), () => null!);
        apiClientMock.Setup(a => a.PostNotification(requestedByMemberId, It.IsAny<PostNotificationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(notificationsResponse);

        //Act
        OkObjectResult result = (OkObjectResult)await sut.CreateNotification(requestedByMemberId, postNotificationRequest, cancellationToken);

        //Assert
        Assert.That(result.Value, Is.Null);
    }

    [Test, MoqAutoData]
    public async Task CreateNotification_ValidCommand_ReturnsOk(
    [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
    [Greedy] NotificationsController sut,
    PostNotificationRequest postNotificationRequest,
    Guid requestedByMemberId,
    CancellationToken cancellationToken)
    {
        //Arrange
        Response<GetNotificationResponse> notificationsResponse = new Response<GetNotificationResponse>(string.Empty, new(HttpStatusCode.Created), () => null!);
        apiClientMock.Setup(a => a.PostNotification(requestedByMemberId, It.IsAny<PostNotificationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(notificationsResponse);

        //Act
        var response = await sut.CreateNotification(requestedByMemberId, postNotificationRequest, cancellationToken);
        var actualObjectResult = response as ObjectResult;

        //Assert
        using (new AssertionScope())
        {
            apiClientMock.Verify(a => a.PostNotification(requestedByMemberId, It.IsAny<PostNotificationRequest>(), It.IsAny<CancellationToken>()));
            actualObjectResult.Should().BeOfType<OkObjectResult>();
        }
    }
}

