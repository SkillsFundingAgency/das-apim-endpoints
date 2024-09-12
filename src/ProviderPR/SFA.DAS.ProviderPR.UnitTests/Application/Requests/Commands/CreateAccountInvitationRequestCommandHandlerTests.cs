using System.Net;
using AutoFixture.NUnit3;
using Moq;
using RestEase;
using SFA.DAS.ProviderPR.Application.Requests.Commands.AccountInvitation;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Notifications.Commands;

namespace SFA.DAS.ProviderPR.UnitTests.Application.Requests.Commands;

public class CreateAccountInvitationRequestCommandHandlerTests
{
    private static Mock<IProviderRelationshipsApiRestClient> _providerRelationshipsApiRestClient;

    [SetUp]
    public void SetUp()
    {
        _providerRelationshipsApiRestClient = new Mock<IProviderRelationshipsApiRestClient>();
    }

    [Test]
    [AutoData]
    public async Task Handle_CreateAccount_SendsNotification(
        CreateAccountInvitationRequestCommand command,
        CreateAccountInvitationRequestCommandResult response
    )
    {
        _providerRelationshipsApiRestClient.Setup(x =>
            x.CreateAccountInvitationRequest(
                It.IsAny<CreateAccountInvitationRequestCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(response);

        CreateAccountInvitationRequestCommandHandler sut = CreateHandler();
        var result = await sut.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RequestId, Is.EqualTo(response.RequestId));
        });

        _providerRelationshipsApiRestClient.Verify(a => a.PostNotifications(It.IsAny<PostNotificationsCommand>(), CancellationToken.None), Times.Once);
    }

    [Test]
    [AutoData]
    public void Handle_CreateAccount_ThrowsException_NoNotificationSent(
        CreateAccountInvitationRequestCommand command,
        CreateAccountInvitationRequestCommandResult response
    )
    {
        _providerRelationshipsApiRestClient.Setup(x =>
            x.CreateAccountInvitationRequest(
                It.IsAny<CreateAccountInvitationRequestCommand>(),
                CancellationToken.None
            )
        ).ThrowsAsync(
            new ApiException(
                new HttpRequestMessage(),
                new HttpResponseMessage(HttpStatusCode.BadRequest),
                "RestEase.ApiException: POST failed because response status code does not indicate success: 400(Bad Request)."
            )
        );

        CreateAccountInvitationRequestCommandHandler sut = CreateHandler();

        Assert.ThrowsAsync<ApiException>(async () => await sut.Handle(command, CancellationToken.None));

        _providerRelationshipsApiRestClient.Verify(a => a.PostNotifications(It.IsAny<PostNotificationsCommand>(), CancellationToken.None), Times.Never);
    }

    private static CreateAccountInvitationRequestCommandHandler CreateHandler()
    {
        return new(_providerRelationshipsApiRestClient.Object);
    }
}
