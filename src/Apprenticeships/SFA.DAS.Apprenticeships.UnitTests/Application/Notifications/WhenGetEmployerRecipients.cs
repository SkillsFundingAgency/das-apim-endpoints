using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Application.Notifications;
using SFA.DAS.Encoding;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.UnitTests.Application.Notifications
{
    [TestFixture]
    internal class WhenGetEmployerRecipients
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
        public async Task ShouldReturnRecipients()
        {
            // Arrange
            var accountId = _fixture.Create<long>();
            var response = _fixture.Create<GetAccountTeamMembersWhichReceiveNotificationsResponse>();
            foreach(var recipient in response)
            {
                recipient.CanReceiveNotifications = true;
            }

            _accountsApiClientMock
                .Setup(x => x.Get<GetAccountTeamMembersWhichReceiveNotificationsResponse>(It.IsAny<GetAccountTeamMembersWhichReceiveNotificationsRequest>()))
                .ReturnsAsync(response);

            // Act
            var result = await _service.GetEmployerRecipients(accountId);

            // Assert
            result.Should().HaveCount(response.Count);
        }
    }
}