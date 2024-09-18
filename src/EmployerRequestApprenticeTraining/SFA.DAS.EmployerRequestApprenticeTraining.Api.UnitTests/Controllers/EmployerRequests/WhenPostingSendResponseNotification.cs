using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Models;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.SendResponseNotification;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.CanUserReceiveNotifications;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerProfileUser;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequests
{
    public class WhenPostingSendResponseNotification
    {
        [Test, MoqAutoData]
        public async Task Then_IfCanReceiveNotificationsIsTrue_NotificationSent_And_Ok_Is_Returned(
            SendResponseNotificationEmailParameters parameters,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerRequestsController controller,
            GetEmployerProfileUserResult employerUserProfileResult)
        {
            //Arrange
            mockMediator
                .Setup(m => m.Send(It.IsAny<GetEmployerProfileUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(employerUserProfileResult);

            mockMediator
                .Setup(m => m.Send(It.IsAny<CanUserReceiveNotificationsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mockMediator
                .Setup(m => m.Send(It.IsAny<SendResponseNotificationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Unit());

            // Act
            var actual = await controller.SendEmployerRequestResponseNotifications(parameters) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            mockMediator.Verify(m => m.Send(It.IsAny<SendResponseNotificationCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            
        }

        [Test, MoqAutoData]
        public async Task Then_IfCanReceiveNotificationsIsFalse_NotificationNotSent_And_Ok_Is_Returned(
            SendResponseNotificationEmailParameters parameters,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerRequestsController controller,
            GetEmployerProfileUserResult employerUserProfileResult)
        {
            //Arrange
            mockMediator
                .Setup(m => m.Send(It.IsAny<GetEmployerProfileUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(employerUserProfileResult);

            mockMediator
                .Setup(m => m.Send(It.IsAny<CanUserReceiveNotificationsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mockMediator
                .Setup(m => m.Send(It.IsAny<SendResponseNotificationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Unit());

            // Act
            var actual = await controller.SendEmployerRequestResponseNotifications(parameters) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            mockMediator.Verify(m => m.Send(It.IsAny<SendResponseNotificationCommand>(), It.IsAny<CancellationToken>()), Times.Never);

        }

        [Test, MoqAutoData]
        public async Task GetEmployerProfileUserQuery_IsUnsuccessful_Then_ReturnBadRequest
            (SendResponseNotificationEmailParameters param,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestsController controller)
        {
            // Arrange
            mediator
               .Setup(m => m.Send(It.IsAny<GetEmployerProfileUserQuery>(), It.IsAny<CancellationToken>()))
               .Throws(new Exception());

            // Act
            var result = await controller.SendEmployerRequestResponseNotifications(param) as StatusCodeResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

        }

        [Test, MoqAutoData]
        public async Task CanReceiveUserNotificationQuery_IsUnsuccessful_Then_ReturnBadRequest
            (SendResponseNotificationEmailParameters param,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestsController controller,
            GetEmployerProfileUserResult employerUserProfileResult)
        {
            // Arrange
            mediator
               .Setup(m => m.Send(It.IsAny<GetEmployerProfileUserQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(employerUserProfileResult);

            mediator
                .Setup(m => m.Send(It.IsAny<CanUserReceiveNotificationsQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());
            
            // Act
            var result = await controller.SendEmployerRequestResponseNotifications(param) as StatusCodeResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }


        [Test, MoqAutoData]
        public async Task SendResponseNotificationCommand_IsUnsuccessful_Then_ReturnBadRequest
            (SendResponseNotificationEmailParameters param,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestsController controller,
            GetEmployerProfileUserResult employerUserProfileResult)
        {
            // Arrange
            mediator
               .Setup(m => m.Send(It.IsAny<GetEmployerProfileUserQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(employerUserProfileResult);

            mediator
                .Setup(m => m.Send(It.IsAny<CanUserReceiveNotificationsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediator
                .Setup(m => m.Send(It.IsAny<SendResponseNotificationCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await controller.SendEmployerRequestResponseNotifications(param) as StatusCodeResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
