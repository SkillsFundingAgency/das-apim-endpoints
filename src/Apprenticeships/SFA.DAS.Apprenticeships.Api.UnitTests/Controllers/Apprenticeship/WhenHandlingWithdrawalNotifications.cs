using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Controllers;
using SFA.DAS.Apprenticeships.Api.Models;
using SFA.DAS.Apprenticeships.Application.Notifications;
using SFA.DAS.Apprenticeships.Application.Notifications.Handlers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Apprenticeships.Api.UnitTests.Controllers.Apprenticeship
{
    public class WhenHandlingWithdrawalNotifications
    {
        private Mock<IMediator> _mockMediator;
        private ApprenticeshipController _sut;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _mockMediator = new Mock<IMediator>();
            _sut = new ApprenticeshipController(
                Mock.Of<ILogger<ApprenticeshipController>>(),
                Mock.Of<ILearningApiClient<LearningApiConfiguration>>(),
                Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>(),
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
}
