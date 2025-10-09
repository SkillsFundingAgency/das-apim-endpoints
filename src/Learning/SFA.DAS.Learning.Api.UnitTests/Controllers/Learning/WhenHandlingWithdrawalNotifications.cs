using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Learning.Api.Controllers;
using SFA.DAS.Learning.Api.Models;
using SFA.DAS.Learning.Application.Notification;
using SFA.DAS.Learning.Application.Notification.Handlers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Learning.Api.UnitTests.Controllers.Learning;

[TestFixture]
public class WhenHandlingWithdrawalNotifications
{
#pragma warning disable CS8618 //Disable nullable check
    private Mock<IMediator> _mockMediator;
    private LearningController _sut;
    private Fixture _fixture;
#pragma warning restore CS8618

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _mockMediator = new Mock<IMediator>();
        _sut = new LearningController(
            Mock.Of<ILogger<LearningController>>(),
            _mockMediator.Object);
    }

    [Test]
    public async Task IfNotificationSucceeds_ThenReturnOk()
    {
        // Arrange
        var apprenticeshipKey = _fixture.Create<Guid>();
        var request = _fixture.Create<HandleWithdrawalNotificationsRequest>();

        _mockMediator
            .Setup(m => m.Send(It.IsAny<ApprenticeshipWithdrawnCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NotificationResponse { Success = true });

        // Act
        var result = await _sut.HandleWithdrawalNotifications(apprenticeshipKey, request);

        // Assert
        _mockMediator.Verify(m => m.Send(It.Is<ApprenticeshipWithdrawnCommand>(x => x.LastDayOfLearning == request.LastDayOfLearning), It.IsAny<CancellationToken>()), Times.Once);
        result.ShouldBeOfType<OkResult>();
    }

    [Test]
    public async Task IfNotificationFails_ThenReturnBadRequest()
    {
        // Arrange
        var apprenticeshipKey = _fixture.Create<Guid>();
        var request = _fixture.Create<HandleWithdrawalNotificationsRequest>();

        _mockMediator
            .Setup(m => m.Send(It.IsAny<ApprenticeshipWithdrawnCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new NotificationResponse { Success = false });

        // Act
        var result = await _sut.HandleWithdrawalNotifications(apprenticeshipKey, request);

        // Assert
        result.ShouldBeOfType<BadRequestResult>();
    }
}
