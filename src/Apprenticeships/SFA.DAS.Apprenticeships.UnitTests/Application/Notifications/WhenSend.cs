using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Application.Notifications;
using SFA.DAS.Encoding;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.UnitTests.Application.Notifications
{
    [TestFixture]
    public class WhenSend
    {
#pragma warning disable CS8618 //Disable nullable check
        private Fixture _fixture;
        private Mock<ILogger<ExtendedNotificationService>> _loggerMock;
        private Mock<IAccountsApiClient<AccountsConfiguration>> _accountsApiClientMock;
        private Mock<ILearningApiClient<LearningApiConfiguration>> _apprenticeshipsApiClientMock;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiCommitmentsClientMock;
        private Mock<IEncodingService> _encodingServiceMock;
        private Mock<INotificationService> _notificationServiceMock;
        private ExtendedNotificationService _service;
#pragma warning restore CS8618

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _loggerMock = new Mock<ILogger<ExtendedNotificationService>>();
            _accountsApiClientMock = new Mock<IAccountsApiClient<AccountsConfiguration>>();
            _apprenticeshipsApiClientMock = new Mock<ILearningApiClient<LearningApiConfiguration>>();
            _apiCommitmentsClientMock = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
            _encodingServiceMock = new Mock<IEncodingService>();
            _notificationServiceMock = new Mock<INotificationService>();

            _service = new ExtendedNotificationService(
                _loggerMock.Object,
                _accountsApiClientMock.Object,
                _apprenticeshipsApiClientMock.Object,
                _apiCommitmentsClientMock.Object,
                _encodingServiceMock.Object,
                _notificationServiceMock.Object, 
                Mock.Of<IProviderAccountApiClient<ProviderAccountApiConfiguration>>());
        }


        [Test]
        public async Task ShouldReturnTrue_WhenEmailIsSent()
        {
            // Arrange
            var recipient = _fixture.Create<Recipient>();
            var templateId = _fixture.Create<string>();
            var tokens = _fixture.Create<Dictionary<string, string>>();

            // Act
            var result = await _service.Send(recipient, templateId, tokens);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task Send_ShouldReturnFalse_WhenExceptionIsThrown()
        {
            // Arrange
            var recipient = _fixture.Create<Recipient>();
            var templateId = _fixture.Create<string>();
            var tokens = _fixture.Create<Dictionary<string, string>>();
            _notificationServiceMock
                .Setup(x => x.Send(It.IsAny<SendEmailCommand>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _service.Send(recipient, templateId, tokens);

            // Assert
            result.Should().BeFalse();
        }
    }
}
