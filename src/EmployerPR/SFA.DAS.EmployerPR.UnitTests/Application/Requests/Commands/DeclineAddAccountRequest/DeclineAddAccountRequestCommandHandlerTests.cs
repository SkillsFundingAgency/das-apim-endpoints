﻿using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.Requests.Commands.DeclineAddAccountRequest;
using SFA.DAS.EmployerPR.Application.Requests.Commands.DeclinePermissionsRequest;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.EmployerPR.InnerApi.Requests;

namespace SFA.DAS.EmployerPR.UnitTests.Application.Requests.Commands.DeclineAddAccountRequest;

public sealed class DeclineAddAccountRequestCommandHandlerTests
{
    private Mock<IProviderRelationshipsApiRestClient> _providerRelationshipsApiRestClientMock;
    private DeclineAddAccountRequestCommandHandler _handler;
    private CancellationToken cancellationToken = CancellationToken.None;

    [SetUp]
    public void SetUp()
    {
        _providerRelationshipsApiRestClientMock = new Mock<IProviderRelationshipsApiRestClient>();
        _handler = new DeclineAddAccountRequestCommandHandler(_providerRelationshipsApiRestClientMock.Object);
    }

    [Test]
    public async Task Handle_DeclineAddAccountRequest_CalledOnceWithCorrectParameters()
    {
        var requestId = Guid.NewGuid();
        var actionedBy = requestId.ToString();
        var command = new DeclineAddAccountRequestCommand
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
        var command = new DeclineAddAccountRequestCommand
        {
            RequestId = requestId,
            ActionedBy = actionedBy
        };

        await _handler.Handle(command, cancellationToken);

        _providerRelationshipsApiRestClientMock.Verify(x =>
            x.PostNotifications(
                It.Is<PostNotificationsRequest>(cmd =>
                    cmd.Notifications.Count() == 1 &&
                    cmd.Notifications[0].TemplateName == nameof(PermissionEmailTemplateType.AddAccountDeclined) &&
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
        var command = new DeclineAddAccountRequestCommand
        {
            RequestId = Guid.NewGuid(),
            ActionedBy = Guid.NewGuid().ToString()
        };

        var result = await _handler.Handle(command, cancellationToken);

        Assert.That(result, Is.EqualTo(Unit.Value));
    }
}
