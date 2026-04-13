using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Api.Controllers;
using SFA.DAS.ApprenticeApp.Application.Commands.LearnerNotifications;
using SFA.DAS.ApprenticeApp.Application.Queries.LearnerNotifications;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Controllers
{
    public class LearnerNotificationsControllerTests
    {
        [Test, MoqAutoData]
        public async Task GetLearnerNotifications_Returns_Ok_With_Notifications(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] LearnerNotificationsController controller,
            List<LearnerNotification> notifications)
        {
            // Arrange
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            var accountIdentifier = Guid.NewGuid();
            var expectedResult = new GetLearnerNotificationsQueryResult { Notifications = notifications };

            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetLearnerNotificationsQuery>(q => q.AccountIdentifier == accountIdentifier),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var actionResult = await controller.GetLearnerNotifications(accountIdentifier);

            // Assert
            var ok = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().BeSameAs(notifications);

            mediatorMock.Verify(m => m.Send(
                    It.Is<GetLearnerNotificationsQuery>(q => q.AccountIdentifier == accountIdentifier),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetLearnerNotifications_Returns_NotFound_When_Null(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] LearnerNotificationsController controller)
        {
            // Arrange
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            var accountIdentifier = Guid.NewGuid();
            var expectedResult = new GetLearnerNotificationsQueryResult { Notifications = null };

            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetLearnerNotificationsQuery>(q => q.AccountIdentifier == accountIdentifier),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var actionResult = await controller.GetLearnerNotifications(accountIdentifier);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Test, MoqAutoData]
        public async Task GetLearnerNotificationById_Returns_Ok_With_Notification(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] LearnerNotificationsController controller,
            LearnerNotification notification)
        {
            // Arrange
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            var accountIdentifier = Guid.NewGuid();
            long notificationIdentifier = 12345L;
            var expectedResult = new GetLearnerNotificationByIdQueryResult { Notification = notification };

            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetLearnerNotificationByIdQuery>(q =>
                        q.AccountIdentifier == accountIdentifier &&
                        q.NotificationIdentifier == notificationIdentifier),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var actionResult = await controller.GetLearnerNotificationById(accountIdentifier, notificationIdentifier);

            // Assert
            var ok = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().BeSameAs(notification);

            mediatorMock.Verify(m => m.Send(
                    It.Is<GetLearnerNotificationByIdQuery>(q =>
                        q.AccountIdentifier == accountIdentifier &&
                        q.NotificationIdentifier == notificationIdentifier),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetLearnerNotificationById_Returns_NotFound_When_Null(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] LearnerNotificationsController controller)
        {
            // Arrange
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            var accountIdentifier = Guid.NewGuid();
            long notificationIdentifier = 12345L;
            var expectedResult = new GetLearnerNotificationByIdQueryResult { Notification = null };

            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetLearnerNotificationByIdQuery>(q =>
                        q.AccountIdentifier == accountIdentifier &&
                        q.NotificationIdentifier == notificationIdentifier),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var actionResult = await controller.GetLearnerNotificationById(accountIdentifier, notificationIdentifier);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Test, MoqAutoData]
        public async Task GetLearnerNotificationStatus_Returns_Ok_With_Status(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] LearnerNotificationsController controller,
            LearnerNotificationStatus notificationStatus)
        {
            // Arrange
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            var accountIdentifier = Guid.NewGuid();
            long notificationIdentifier = 12345;
            var expectedResult = new GetLearnerNotificationStatusQueryResult { NotificationStatus = notificationStatus };

            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetLearnerNotificationStatusQuery>(q =>
                        q.AccountIdentifier == accountIdentifier &&
                        q.NotificationIdentifier == notificationIdentifier),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var actionResult = await controller.GetLearnerNotificationStatus(accountIdentifier, notificationIdentifier);

            // Assert
            var ok = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().BeSameAs(notificationStatus);

            mediatorMock.Verify(m => m.Send(
                    It.Is<GetLearnerNotificationStatusQuery>(q =>
                        q.AccountIdentifier == accountIdentifier &&
                        q.NotificationIdentifier == notificationIdentifier),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetLearnerNotificationStatus_Returns_NotFound_When_Null(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] LearnerNotificationsController controller)
        {
            // Arrange
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            var accountIdentifier = Guid.NewGuid();
            long notificationIdentifier = 12345L;
            var expectedResult = new GetLearnerNotificationStatusQueryResult { NotificationStatus = null };

            mediatorMock
                .Setup(m => m.Send(
                    It.Is<GetLearnerNotificationStatusQuery>(q =>
                        q.AccountIdentifier == accountIdentifier &&
                        q.NotificationIdentifier == notificationIdentifier),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var actionResult = await controller.GetLearnerNotificationStatus(accountIdentifier, notificationIdentifier);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Test, MoqAutoData]
        public async Task UpdateLearnerNotificationStatus_Returns_Ok_And_Sends_Command(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] LearnerNotificationsController controller)
        {
            // Arrange
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

            var accountIdentifier = Guid.NewGuid();
            long notificationIdentifier = 12345L;
            var request = new UpdateNotificationStatusRequest { Status = "Test" };

            mediatorMock
                .Setup(m => m.Send(
                    It.IsAny<UpdateLearnerNotificationStatusCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var actionResult = await controller.UpdateLearnerNotificationStatus(accountIdentifier, notificationIdentifier, request);

            // Assert
            actionResult.Should().BeOfType<OkResult>();

            mediatorMock.Verify(m => m.Send(
                    It.Is<UpdateLearnerNotificationStatusCommand>(c =>
                        c.AccountIdentifier == accountIdentifier &&
                        c.NotificationIdentifier == notificationIdentifier &&
                        c.Status == request.Status),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}