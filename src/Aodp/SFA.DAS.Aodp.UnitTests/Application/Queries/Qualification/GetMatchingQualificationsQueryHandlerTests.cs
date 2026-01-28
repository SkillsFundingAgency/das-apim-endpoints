using Moq;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Qualification;

[TestFixture]
public class GetMatchingQualificationsQueryHandlerTests
{
    private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock = null!;
    private GetMatchingQualificationsQueryHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _apiClientMock = new Mock<IAodpApiClient<AodpApiConfiguration>>();
        _handler = new GetMatchingQualificationsQueryHandler(_apiClientMock.Object);
    }

    [Test]
    public async Task Handle_ReturnsSuccessAndMapsValues_WhenApiReturnsQualifications()
    {
        // Arrange
        var items = new List<GetMatchingQualificationsQueryItem>
            {
                new GetMatchingQualificationsQueryItem()
            };

        var apiResult = new GetMatchingQualificationsQueryResponse
        {
            Qualifications = items,
            TotalRecords = 1,
            Skip = 0,
            Take = 10
        };

        _apiClientMock
            .Setup(c => c.Get<GetMatchingQualificationsQueryResponse>(It.IsAny<GetMatchingQualificationsQueryApiRequest>()))
            .ReturnsAsync(apiResult);

        var request = new GetMatchingQualificationsQuery("term", 0, 10);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(items, Is.EqualTo(result.Value.Qualifications));
        Assert.That(1, Is.EqualTo(result.Value.TotalRecords));
        Assert.That(0, Is.EqualTo(result.Value.Skip));
        Assert.That(10, Is.EqualTo(result.Value.Take));
    }

    [Test]
    public async Task Handle_ReturnsNoMatchesMessage_WhenApiReturnsNullQualifications()
    {
        // Arrange
        var apiResult = new GetMatchingQualificationsQueryResponse
        {
            Qualifications = null!, 
            TotalRecords = 0,
            Skip = 0,
            Take = 10
        };

        _apiClientMock
            .Setup(c => c.Get<GetMatchingQualificationsQueryResponse>(It.IsAny<GetMatchingQualificationsQueryApiRequest>()))
            .ReturnsAsync(apiResult);

        var request = new GetMatchingQualificationsQuery("term", 0, 10);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.ErrorMessage, Is.EqualTo("No matching qualifications found."));
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.Qualifications, Is.Not.Null);
        Assert.That(result.Value.Qualifications, Is.Empty);
    }

    [Test]
    public async Task Handle_ReturnsNoMatchesMessage_WhenApiReturnsNullResult()
    {
        // Arrange
        _apiClientMock
            .Setup(c => c.Get<GetMatchingQualificationsQueryResponse>(It.IsAny<GetMatchingQualificationsQueryApiRequest>()))
            .ReturnsAsync((GetMatchingQualificationsQueryResponse)null!);

        var request = new GetMatchingQualificationsQuery("term", 0, 10);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.ErrorMessage, Is.EqualTo("No matching qualifications found."));
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.Qualifications, Is.Not.Null);
        Assert.That(result.Value.Qualifications, Is.Empty);
    }

    [Test]
    public async Task Handle_ReturnsFailureAndErrorMessage_WhenApiThrows()
    {
        // Arrange
        _apiClientMock
            .Setup(c => c.Get<GetMatchingQualificationsQueryResponse>(It.IsAny<GetMatchingQualificationsQueryApiRequest>()))
            .ThrowsAsync(new InvalidOperationException("Boom"));

        var request = new GetMatchingQualificationsQuery("term", 0, 10);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.ErrorMessage, Is.EqualTo("Boom"));
    }
}