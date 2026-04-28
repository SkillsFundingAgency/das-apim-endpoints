using Moq;
using SFA.DAS.Aodp.Application.Queries.Rollover;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Rollover;

[TestFixture]
public class GetRolloverWorkflowCandidatesCountQueryHandlerTests
{
    private Mock<IAodpApiClient<AodpApiConfiguration>> _mockApiClient = null!;
    private GetRolloverWorkflowCandidatesCountQueryHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _mockApiClient = new Mock<IAodpApiClient<AodpApiConfiguration>>();
        _handler = new GetRolloverWorkflowCandidatesCountQueryHandler(_mockApiClient.Object);
    }

    [Test]
    public async Task Handle_WhenApiReturnsSuccessfulResponse_ShouldReturnSuccessAndTotalRecords()
    {
        // Arrange
        var apiResponse = new BaseMediatrResponse<GetRolloverWorkflowCandidatesCountQueryResponse>
        {
            Value = new GetRolloverWorkflowCandidatesCountQueryResponse { TotalRecords = 7 },
            Success = true
        };

        _mockApiClient
            .Setup(c => c.Get<BaseMediatrResponse<GetRolloverWorkflowCandidatesCountQueryResponse>>(It.IsAny<IGetApiRequest>()))
            .ReturnsAsync(apiResponse);

        var query = new GetRolloverWorkflowCandidatesCountQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Success, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.TotalRecords, Is.EqualTo(7));

        _mockApiClient.Verify(c => c.Get<BaseMediatrResponse<GetRolloverWorkflowCandidatesCountQueryResponse>>(It.IsAny<IGetApiRequest>()), Times.Once);
    }

    [Test]
    public async Task Handle_WhenApiThrowsException_ShouldReturnFailureWithExceptionMessage()
    {
        // Arrange
        var ex = new InvalidOperationException("api failure");
        _mockApiClient
            .Setup(c => c.Get<BaseMediatrResponse<GetRolloverWorkflowCandidatesCountQueryResponse>>(It.IsAny<IGetApiRequest>()))
            .ThrowsAsync(ex);

        var query = new GetRolloverWorkflowCandidatesCountQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Success, Is.False);
        Assert.That(result.ErrorMessage, Is.EqualTo("api failure"));

        _mockApiClient.Verify(c => c.Get<BaseMediatrResponse<GetRolloverWorkflowCandidatesCountQueryResponse>>(It.IsAny<IGetApiRequest>()), Times.Once);
    }
}