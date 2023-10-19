using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestEase;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.InnerApi.Notifications;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers;

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
        notificationsResponse.MemberId = requestedByMemberId;
        apiClientMock.Setup(a => a.GetNotification(notificationId, requestedByMemberId, It.IsAny<CancellationToken>())).ReturnsAsync(new Response<GetNotificationResponse?>(string.Empty, new(HttpStatusCode.NotFound), () => null));

        var actual = await sut.GetNotification(notificationId, requestedByMemberId, It.IsAny<CancellationToken>());
        var actualObjectResult = actual as ObjectResult;

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
        notificationsResponse.MemberId = requestedByMemberId;
        apiClientMock.Setup(a => a.GetNotification(notificationId, requestedByMemberId, It.IsAny<CancellationToken>())).ReturnsAsync(new Response<GetNotificationResponse?>(string.Empty, new(HttpStatusCode.OK), () => notificationsResponse));

        var actual = await sut.GetNotification(notificationId, requestedByMemberId, It.IsAny<CancellationToken>());
        var actualObjectResult = actual as ObjectResult;

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
        notificationsResponse.MemberId = requestedByMemberId;
        apiClientMock.Setup(a => a.GetNotification(notificationId, requestedByMemberId, It.IsAny<CancellationToken>())).ReturnsAsync(new Response<GetNotificationResponse?>(string.Empty, new(HttpStatusCode.Unauthorized), () => null));

        Func<Task> actual = async () => await sut.GetNotification(notificationId, requestedByMemberId, It.IsAny<CancellationToken>());

        using (new AssertionScope())
        {
            await actual.Should().ThrowAsync<InvalidOperationException>().WithMessage("Get notification didn't come back with a successful response");
            apiClientMock.Verify(a => a.GetNotification(notificationId, requestedByMemberId, It.IsAny<CancellationToken>()));
        }
    }
}

