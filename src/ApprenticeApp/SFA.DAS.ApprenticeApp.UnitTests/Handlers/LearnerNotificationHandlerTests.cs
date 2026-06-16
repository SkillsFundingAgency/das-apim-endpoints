using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Commands.LearnerNotifications;
using SFA.DAS.ApprenticeApp.Application.Queries.LearnerNotifications;
using SFA.DAS.ApprenticeApp.InnerApi.LearnerNotifications.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class LearnerNotificationHandlerTests
    {
        [Test, MoqAutoData]
        public async Task GetLearnerNotificationsQueryHandler_Returns_Notifications(
            [Frozen] Mock<ILearnerNotificationsInnerApiClient<LearnerNotificationsApiConfiguration>> apiClientMock,
            GetLearnerNotificationsQueryHandler sut)
        {
            // Arrange
            var query = new GetLearnerNotificationsQuery
            {
                AccountIdentifier = Guid.NewGuid()
            };

            var notifications = new List<LearnerNotification>
            {
                new LearnerNotification
                {
                    NotificationId = 1,
                    Heading = "Test Notification",
                    Category = "General"
                }
            };

            var apiResponse = new GetLearnerNotificationsQueryResult
            {
                Notifications = notifications
            };

            apiClientMock
                .Setup(c => c.Get<GetLearnerNotificationsQueryResult>(
                    It.Is<GetLearnerNotificationsRequest>(r =>
                        r.GetUrl == $"learner/{query.AccountIdentifier}")))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Notifications.Should().HaveCount(1);
            result.Notifications[0].NotificationId.Should().Be(1);
            result.Notifications[0].Heading.Should().Be("Test Notification");

            apiClientMock.Verify(c => c.Get<GetLearnerNotificationsQueryResult>(
                It.IsAny<GetLearnerNotificationsRequest>()),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetLearnerNotificationByIdQueryHandler_Returns_Notification(
            [Frozen] Mock<ILearnerNotificationsInnerApiClient<LearnerNotificationsApiConfiguration>> apiClientMock,
            GetLearnerNotificationByIdQueryHandler sut)
        {
            // Arrange
            var query = new GetLearnerNotificationByIdQuery
            {
                AccountIdentifier = Guid.NewGuid(),
                NotificationIdentifier = 12345L
            };

            var notification = new LearnerNotification
            {
                NotificationId = 12345L,
                Heading = "Test Notification",
                Category = "General"
            };

            var apiResponse = new GetLearnerNotificationByIdQueryResult
            {
                Notification = notification
            };

            apiClientMock
                .Setup(c => c.Get<GetLearnerNotificationByIdQueryResult>(
                    It.Is<GetLearnerNotificationByIdRequest>(r =>
                        r.GetUrl == $"learner/{query.AccountIdentifier}/notifications/{query.NotificationIdentifier}")))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Notification.Should().NotBeNull();
            result.Notification.NotificationId.Should().Be(12345L);
            result.Notification.Heading.Should().Be("Test Notification");

            apiClientMock.Verify(c => c.Get<GetLearnerNotificationByIdQueryResult>(
                It.IsAny<GetLearnerNotificationByIdRequest>()),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task GetLearnerNotificationStatusQueryHandler_Returns_Status(
            [Frozen] Mock<ILearnerNotificationsInnerApiClient<LearnerNotificationsApiConfiguration>> apiClientMock,
            GetLearnerNotificationStatusQueryHandler sut)
        {
            // Arrange
            var query = new GetLearnerNotificationStatusQuery
            {
                AccountIdentifier = Guid.NewGuid(),
                NotificationIdentifier = 12345L
            };

            var notificationStatus = new LearnerNotificationStatus
            {
                StatusId = 0,
                StatusName = "Pending",
                LastUpdated = new DateTime(2025, 12, 13, 18, 02, 35)
            };

            var apiResponse = new GetLearnerNotificationStatusQueryResult
            {
                NotificationStatus = notificationStatus
            };

            apiClientMock
                .Setup(c => c.Get<GetLearnerNotificationStatusQueryResult>(
                    It.Is<GetLearnerNotificationStatusRequest>(r =>
                        r.GetUrl == $"learner/{query.AccountIdentifier}/notifications/{query.NotificationIdentifier}/status")))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await sut.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.NotificationStatus.Should().NotBeNull();
            result.NotificationStatus.StatusId.Should().Be(0);
            result.NotificationStatus.StatusName.Should().Be("Pending");

            apiClientMock.Verify(c => c.Get<GetLearnerNotificationStatusQueryResult>(
                It.IsAny<GetLearnerNotificationStatusRequest>()),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task UpdateLearnerNotificationStatusCommandHandler_Calls_Put(
            [Frozen] Mock<ILearnerNotificationsInnerApiClient<LearnerNotificationsApiConfiguration>> apiClientMock,
            UpdateLearnerNotificationStatusCommandHandler sut)
        {
            // Arrange
            var command = new UpdateLearnerNotificationStatusCommand
            {
                AccountIdentifier = Guid.NewGuid(),
                NotificationIdentifier = 12345L,
                Status = "Read"
            };

            apiClientMock
                .Setup(c => c.Put(It.IsAny<UpdateLearnerNotificationStatusRequest>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);

            apiClientMock.Verify(c => c.Put(
                It.Is<UpdateLearnerNotificationStatusRequest>(r =>
                    r.PutUrl == $"learner/{command.AccountIdentifier}/notifications/{command.NotificationIdentifier}/status" &&
                    r.Data.Status == "Read")),
                Times.Once);
        }
    }
}