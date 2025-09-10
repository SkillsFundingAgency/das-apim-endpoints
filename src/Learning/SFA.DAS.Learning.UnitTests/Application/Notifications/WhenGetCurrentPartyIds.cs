using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Encoding;
using SFA.DAS.Learning.Application.Notification;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.Learning.UnitTests.Application.Notifications;

[TestFixture]
internal class WhenGetCurrentPartyIds
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
    public async Task ShouldReturnPartyIds()
    {
        // Arrange
        var apprenticeshipKey = Guid.NewGuid();
        var expectedResponse = _fixture.Create<GetCurrentPartyIdsResponse>();
        _apprenticeshipsApiClientMock
            .Setup(x => x.Get<GetCurrentPartyIdsResponse>(It.IsAny<GetCurrentPartyIdsRequest>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _service.GetCurrentPartyIds(apprenticeshipKey);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);

    }

    [Test]
    public void ShouldThrowException_WhenResponseIsNull()
    {
        // Arrange
        var apprenticeshipKey = Guid.NewGuid();

#pragma warning disable CS8620, CS8600 // this is intentional to allow the mock to return null
        _apprenticeshipsApiClientMock
            .Setup(x => x.Get<GetCurrentPartyIdsResponse>(It.IsAny<GetCurrentPartyIdsRequest>()))
            .ReturnsAsync((GetCurrentPartyIdsResponse)null);
#pragma warning restore CS8620, CS8600

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() => _service.GetCurrentPartyIds(apprenticeshipKey));
    }
}
