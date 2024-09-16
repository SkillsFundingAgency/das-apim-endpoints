using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.Notifications.Commands.PostNotifications;
using SFA.DAS.EmployerPR.Application.Requests.Commands.DeclinedRequest;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;

namespace SFA.DAS.EmployerPR.UnitTests.Application.Requests.Commands.DeclinedRequest;

public sealed class DeclinePermissionRequestCommandHandlerTests
{
    private Mock<IProviderRelationshipsApiRestClient> _providerRelationshipsApiRestClientMock;
    private DeclinePermissionRequestCommandHandler _handler;
    private CancellationToken cancellationToken = CancellationToken.None;

    [SetUp]
    public void SetUp()
    {
        _providerRelationshipsApiRestClientMock = new Mock<IProviderRelationshipsApiRestClient>();
        _handler = new DeclinePermissionRequestCommandHandler(_providerRelationshipsApiRestClientMock.Object);
    }

    [Test]
    public async Task Handle_DeclinePermissionRequest_CalledOnceWithCorrectParameters()
    {
        var requestId = Guid.NewGuid();
        var actionedBy = requestId.ToString();
        var command = new DeclinePermissionRequestCommand
        {
            RequestId = requestId,
            ActionedBy = actionedBy
        };

        await _handler.Handle(command, cancellationToken);

        _providerRelationshipsApiRestClientMock.Verify(x =>
            x.DeclineRequest(
                requestId,
                It.Is<DeclinedRequestModel>(m => m.ActionedBy == actionedBy),
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
        var command = new DeclinePermissionRequestCommand
        {
            RequestId = requestId,
            ActionedBy = actionedBy
        };

        await _handler.Handle(command, cancellationToken);

        _providerRelationshipsApiRestClientMock.Verify(x =>
            x.PostNotifications(
                It.Is<PostNotificationsCommand>(cmd =>
                    cmd.Notifications.Count() == 1 &&
                    cmd.Notifications[0].TemplateName == nameof(PermissionEmailTemplateType.UpdatePermissionDeclined) &&
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
        var command = new DeclinePermissionRequestCommand
        {
            RequestId = Guid.NewGuid(),
            ActionedBy = Guid.NewGuid().ToString()
        };

        var result = await _handler.Handle(command, cancellationToken);

        Assert.That(result, Is.EqualTo(Unit.Value));
    }
}