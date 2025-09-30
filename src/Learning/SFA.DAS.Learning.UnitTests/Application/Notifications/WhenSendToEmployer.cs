using AutoFixture;
using FluentAssertions;
using Moq;
using SFA.DAS.Learning.Application.Notification;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Learning.UnitTests.Application.Notifications;

[TestFixture]
public class WhenSendToEmployer
{
#pragma warning disable CS8618 //Disable nullable check
    private Fixture _fixture;
    private Mock<IExtendedNotificationService> _notificationServiceMock;
    private TestEmployerNotificationCommandHandler _handler;
#pragma warning restore CS8618

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _notificationServiceMock = new Mock<IExtendedNotificationService>();
        _handler = new TestEmployerNotificationCommandHandler(_notificationServiceMock.Object);
    }

    [Test]
    public async Task SendToEmployer_ShouldSendNotificationsToAllRecipients()
    {
        // Arrange
        var request = _fixture.Create<TestEmployerNotificationCommand>();
        var currentPartyIds = _fixture.Create<GetCurrentPartyIdsResponse>();
        var recipients = _fixture.CreateMany<Recipient>(3).ToList();
        var apprenticeshipDetails = _fixture.Create<CommitmentsApprenticeshipDetails>();

        _notificationServiceMock.Setup(s => s.GetCurrentPartyIds(It.IsAny<Guid>())).ReturnsAsync(currentPartyIds);
        _notificationServiceMock.Setup(s => s.GetEmployerRecipients(It.IsAny<long>())).ReturnsAsync(recipients);
        _notificationServiceMock.Setup(s => s.GetApprenticeship(It.IsAny<GetCurrentPartyIdsResponse>())).ReturnsAsync(apprenticeshipDetails);
        _notificationServiceMock.Setup(s => s.Send(It.IsAny<Recipient>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).ReturnsAsync(true);

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Success.Should().BeTrue();
        _notificationServiceMock.Verify(s => s.Send(It.IsAny<Recipient>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Exactly(3));
    }
}

internal class TestEmployerNotificationCommand : NotificationCommandBase { }

internal class TestEmployerNotificationCommandHandler : NotificationCommandHandlerBase<TestEmployerNotificationCommand>
{
    public TestEmployerNotificationCommandHandler(IExtendedNotificationService notificationService) : base(notificationService) { }

    public override async Task<NotificationResponse> Handle(TestEmployerNotificationCommand request, CancellationToken cancellationToken)
    {
        return await SendToEmployer(request, "templateId", (_, __) => GetEmployerTokens());
    }

    private Dictionary<string, string> GetEmployerTokens()
    {
        return new Dictionary<string, string>();
    }
}
