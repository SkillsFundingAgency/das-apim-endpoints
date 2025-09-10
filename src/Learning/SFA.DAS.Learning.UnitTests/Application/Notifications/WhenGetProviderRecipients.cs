using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Encoding;
using SFA.DAS.Learning.Application.Notification;
using SFA.DAS.Learning.InnerApi;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Learning.UnitTests.Application.Notifications;

[TestFixture]
internal class WhenGetProviderRecipients
{
#pragma warning disable CS8618 //Disable nullable check
    private Fixture _fixture;
    private Mock<ILogger<ExtendedNotificationService>> _loggerMock;
    private Mock<IAccountsApiClient<AccountsConfiguration>> _accountsApiClientMock;
    private Mock<ILearningApiClient<LearningApiConfiguration>> _apprenticeshipsApiClientMock;
    private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiCommitmentsClientMock;
    private Mock<IEncodingService> _encodingServiceMock;
    private Mock<INotificationService> _notificationServiceMock;
    private Mock<IProviderAccountApiClient<ProviderAccountApiConfiguration>> _providerAccountMock;
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
        _providerAccountMock = new Mock<IProviderAccountApiClient<ProviderAccountApiConfiguration>>();

        _service = new ExtendedNotificationService(
            _loggerMock.Object,
            _accountsApiClientMock.Object,
            _apprenticeshipsApiClientMock.Object,
            _apiCommitmentsClientMock.Object,
            _encodingServiceMock.Object,
            _notificationServiceMock.Object,
            _providerAccountMock.Object);
    }

    [Test]
    public async Task ShouldReturnRecipients()
    {
        // Arrange
        var providerId = _fixture.Create<long>();
        var response = _fixture.CreateMany<GetProviderUsersListItem>();
        foreach (var recipient in response)
        {
            recipient.ReceiveNotifications = true;
        }

        _providerAccountMock
            .Setup(x => x.GetAll<GetProviderUsersListItem>(It.IsAny<GetProviderUsersRequest>()))
            .ReturnsAsync(response);

        // Act
        var result = await _service.GetProviderRecipients(providerId);

        // Assert
        result.Should().HaveCount(response.Count());
    }
}
