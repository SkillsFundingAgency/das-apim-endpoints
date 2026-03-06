using Moq;
using SFA.DAS.Aodp.Application.Queries.Rollover;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Rollover;

[TestFixture]
public class GetRolloverWorkflowCandidatesQueryHandlerTests
{
    private Mock<IAodpApiClient<AodpApiConfiguration>> _mockApiClient = null!;
    private GetRolloverWorkflowCandidatesQueryHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _mockApiClient = new Mock<IAodpApiClient<AodpApiConfiguration>>();
        _handler = new GetRolloverWorkflowCandidatesQueryHandler(_mockApiClient.Object);
    }

    [Test]
    public async Task Handle_WhenApiReturnsResponse_ShouldReturnSuccessAndValue()
    {
        // Arrange
        var expected = new GetRolloverWorkflowCandidatesQueryResponse
        {
            TotalRecords = 1
        };

        _mockApiClient
            .Setup(c => c.Get<GetRolloverWorkflowCandidatesQueryResponse>(It.IsAny<IGetApiRequest>()))
            .ReturnsAsync(expected);

        var query = new GetRolloverWorkflowCandidatesQuery(5, 10);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Success, Is.True);
        Assert.That(result.Value, Is.SameAs(expected));

        _mockApiClient.Verify(c => c.Get<GetRolloverWorkflowCandidatesQueryResponse>(
            It.Is<IGetApiRequest>(r =>
                r.GetType().GetProperty("Skip") != null &&
                (int?)r.GetType().GetProperty("Skip").GetValue(r) == 5 &&
                r.GetType().GetProperty("Take") != null &&
                (int?)r.GetType().GetProperty("Take").GetValue(r) == 10
            )), Times.Once);
    }

    [Test]
    public async Task Handle_WhenApiReturnsNull_ShouldReturnFailureWithExpectedErrorMessage()
    {
        // Arrange
        _mockApiClient
            .Setup(c => c.Get<GetRolloverWorkflowCandidatesQueryResponse>(It.IsAny<IGetApiRequest>()))
            .ReturnsAsync((GetRolloverWorkflowCandidatesQueryResponse?)null);

        var query = new GetRolloverWorkflowCandidatesQuery(null, null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Success, Is.False);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.Data, Is.Empty);
        Assert.That(result.Value.TotalRecords, Is.EqualTo(0));
        Assert.That(result.ErrorMessage, Is.EqualTo("Failed to get rollover workflow candidates."));
    }

    [Test]
    public async Task Handle_WhenApiThrowsException_ShouldReturnFailureWithExceptionMessage()
    {
        // Arrange
        var ex = new InvalidOperationException("error");
        _mockApiClient
            .Setup(c => c.Get<GetRolloverWorkflowCandidatesQueryResponse>(It.IsAny<IGetApiRequest>()))
            .ThrowsAsync(ex);

        var query = new GetRolloverWorkflowCandidatesQuery(1, 2);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Success, Is.False);
        Assert.That(result.ErrorMessage, Is.EqualTo("error"));
    }
}
