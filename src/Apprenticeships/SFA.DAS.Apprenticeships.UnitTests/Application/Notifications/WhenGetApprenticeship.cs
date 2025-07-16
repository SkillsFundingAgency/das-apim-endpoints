using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Application.Notifications;
using SFA.DAS.Encoding;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;

namespace SFA.DAS.Apprenticeships.UnitTests.Application.Notifications
{
    [TestFixture]
    internal class WhenGetApprenticeship
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
        public async Task ShouldReturnApprenticeshipDetails()
        {
            // Arrange
            var currentPartyIds = _fixture.Create<GetCurrentPartyIdsResponse>();
            var response = _fixture.Create<GetApprenticeshipResponse>();
            _apiCommitmentsClientMock
                .Setup(x => x.GetWithResponseCode<GetApprenticeshipResponse>(It.IsAny<GetApprenticeshipRequest>()))
                .ReturnsAsync(new ApiResponse<GetApprenticeshipResponse>(response, System.Net.HttpStatusCode.OK, ""));

            // Act
            var result = await _service.GetApprenticeship(currentPartyIds);

            // Assert
            result.Should().NotBeNull();
            result.ProviderName.Should().Be(response.ProviderName);
            result.EmployerName.Should().Be(response.EmployerName);
            result.ApprenticeFirstName.Should().Be(response.FirstName);
            result.ApprenticeLastName.Should().Be(response.LastName);

        }

        [Test]
        public void ShouldThrowException_WhenResponseIsNotSuccess()
        {
            // Arrange
            var currentPartyIds = _fixture.Create<GetCurrentPartyIdsResponse>();

#pragma warning disable CS8625 // this is intentional to allow the mock to return null
            _apiCommitmentsClientMock
                .Setup(x => x.GetWithResponseCode<GetApprenticeshipResponse>(It.IsAny<GetApprenticeshipRequest>()))
                .ReturnsAsync(new ApiResponse<GetApprenticeshipResponse>(null, System.Net.HttpStatusCode.NotFound, ""));
#pragma warning restore CS8625

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _service.GetApprenticeship(currentPartyIds));
        }

    }
}
