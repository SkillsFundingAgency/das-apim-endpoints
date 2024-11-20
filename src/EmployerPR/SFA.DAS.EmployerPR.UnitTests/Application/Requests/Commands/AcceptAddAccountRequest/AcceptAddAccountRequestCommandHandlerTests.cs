using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.Requests.Commands.AcceptAddAccountRequest;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.EmployerPR.InnerApi.Requests;

namespace SFA.DAS.EmployerPR.UnitTests.Application.Requests.Commands.AcceptAddAccountRequest;

public sealed class AcceptAddAccountRequestCommandHandlerTests
{
    private Mock<IProviderRelationshipsApiRestClient> _providerRelationshipsApiRestClientMock;
    private AcceptAddAccountRequestCommandHandler _handler;
    private CancellationToken cancellationToken = CancellationToken.None;

    [SetUp]
    public void SetUp()
    {
        _providerRelationshipsApiRestClientMock = new Mock<IProviderRelationshipsApiRestClient>();
        _handler = new AcceptAddAccountRequestCommandHandler(_providerRelationshipsApiRestClientMock.Object);
    }

    [Test]
    public async Task Handle_AcceptAddAccountRequest_CalledOnceWithCorrectParameters()
    {
        var requestId = Guid.NewGuid();
        var actionedBy = requestId.ToString();
        var command = new AcceptAddAccountRequestCommand
        {
            RequestId = requestId,
            ActionedBy = actionedBy
        };

        await _handler.Handle(command, cancellationToken);

        _providerRelationshipsApiRestClientMock.Verify(x =>
            x.AcceptAddAccountRequest(
                requestId,
                It.Is<AcceptAddAccountRequestModel>(m => m.ActionedBy == actionedBy),
                cancellationToken
            ),
            Times.Once
        );
    }

    [Test]
    public async Task Handle_PostNotifications_CalledOnceWithCorrectParameters()
    {
        var requestId = Guid.NewGuid();
        var actionedBy = Guid.NewGuid().ToString();
        var command = new AcceptAddAccountRequestCommand
        {
            RequestId = requestId,
            ActionedBy = actionedBy
        };

        await _handler.Handle(command, cancellationToken);

        _providerRelationshipsApiRestClientMock.Verify(x =>
            x.PostNotifications(
                It.Is<PostNotificationsRequest>(cmd =>
                    cmd.Notifications.Count() == 1 &&
                    cmd.Notifications[0].TemplateName == nameof(PermissionEmailTemplateType.AddAccountAccepted) &&
                    cmd.Notifications[0].NotificationType == nameof(NotificationType.Provider) &&
                    cmd.Notifications[0].RequestId == requestId &&
                    cmd.Notifications[0].CreatedBy == actionedBy
                ),
                cancellationToken
            ),
            Times.Once
        );
    }

    [Test]
    public async Task Handle_Returns_Unit()
    {
        var command = new AcceptAddAccountRequestCommand
        {
            RequestId = Guid.NewGuid(),
            ActionedBy = Guid.NewGuid().ToString()
        };

        var result = await _handler.Handle(command, cancellationToken);

        Assert.That(result, Is.EqualTo(Unit.Value));
    }
}
