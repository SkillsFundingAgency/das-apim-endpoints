using Moq;
using SFA.DAS.Aodp.Application.Queries.Rollover;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;
using SFA.DAS.Aodp.Services;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Rollover;

[TestFixture]
public class GetLevelsForRolloverQueryBuilderQueryHandlerTests
{
    private Mock<IAodpApiClient<AodpApiConfiguration>> _mockApiClient = null!;
    private GetLevelsForRolloverQueryBuilderQueryHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _mockApiClient = new Mock<IAodpApiClient<AodpApiConfiguration>>();
        _handler = new GetLevelsForRolloverQueryBuilderQueryHandler(_mockApiClient.Object);
    }

    [Test]
    public async Task Handle_WhenApiReturnsLevels_ShouldReturnSuccessfulResponse()
    {
        // Arrange
        var apiResponse = new GetLevelsForRolloverQueryBuilderQueryResponse
        {
            Levels = [new RolloverLevel { Id = 3, Name = "Level 3" }]
        };

        _mockApiClient
            .Setup(x => x.Get<GetLevelsForRolloverQueryBuilderQueryResponse>(
                It.IsAny<GetLevelsForRolloverQueryBuilderApiRequest>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _handler.Handle(new GetLevelsForRolloverQueryBuilderQuery(), CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.SameAs(apiResponse));
            _mockApiClient.Verify(x => x.Get<GetLevelsForRolloverQueryBuilderQueryResponse>(
                It.IsAny<GetLevelsForRolloverQueryBuilderApiRequest>()), Times.Once);
        });
    }

    [Test]
    public async Task Handle_WhenApiThrowsException_ShouldReturnFailureResponse()
    {
        // Arrange
        _mockApiClient
            .Setup(x => x.Get<GetLevelsForRolloverQueryBuilderQueryResponse>(
                It.IsAny<GetLevelsForRolloverQueryBuilderApiRequest>()))
            .ThrowsAsync(new InvalidOperationException("Unable to get levels"));

        // Act
        var result = await _handler.Handle(new GetLevelsForRolloverQueryBuilderQuery(), CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Unable to get levels"));
        });
    }
}
