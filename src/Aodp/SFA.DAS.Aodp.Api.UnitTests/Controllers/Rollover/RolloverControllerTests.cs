using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Api.Controllers.Rollover;
using SFA.DAS.Aodp.Application.Queries.Rollover;

namespace SFA.DAS.Aodp.Api.UnitTests.Controllers.Rollover;

[TestFixture]
public class RolloverControllerTests
{
    private Mock<IMediator> _mockMediator = null!;
    private Mock<ILogger<RolloverController>> _mockLogger = null!;

    [SetUp]
    public void SetUp()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<RolloverController>>();
    }

    [Test]
    public async Task GetRolloverWorkflowCandidates_WhenMediatorReturnsSuccess_ShouldReturnOkWithValue()
    {
        // Arrange
        var payload = new GetRolloverWorkflowCandidatesQueryResponse();
        var mediatrResponse = new BaseMediatrResponse<GetRolloverWorkflowCandidatesQueryResponse>
        {
            Success = true,
            Value = payload
        };

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetRolloverWorkflowCandidatesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatrResponse);

        // Act
        var actionResult = await controller.GetRolloverWorkflowCandidates(1, 2);

        // Assert
        Assert.That(actionResult, Is.InstanceOf<OkObjectResult>());
        var ok = (OkObjectResult)actionResult;
        Assert.That(ok.Value, Is.SameAs(payload));
    }

    [Test]
    public async Task GetRolloverWorkflowCandidates_WhenMediatorReturnsFailure_ShouldReturn500AndLogError()
    {
        // Arrange
        var mediatrResponse = new BaseMediatrResponse<GetRolloverWorkflowCandidatesQueryResponse>
        {
            Success = false,
            ErrorMessage = "some error"
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetRolloverWorkflowCandidatesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatrResponse);
        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        // Act
        var actionResult = await controller.GetRolloverWorkflowCandidates(null, null);

        // Assert
        const int expectedStatus = 500;

        if (actionResult is ObjectResult objectResult)
        {
            Assert.That(objectResult.StatusCode, Is.EqualTo(expectedStatus));
        }
        else if (actionResult is StatusCodeResult statusCodeResult)
        {
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(expectedStatus));
        }
        else
        {
            Assert.Fail($"Expected ObjectResult or StatusCodeResult with status {expectedStatus} but got {actionResult?.GetType().FullName ?? "null"}");
        }

        // verify logger.LogError was called with a message containing the mediatr error message
        _mockLogger.Verify(
            x => x.Log(
                It.Is<Microsoft.Extensions.Logging.LogLevel>(l => l == Microsoft.Extensions.Logging.LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v != null && v.ToString().Contains("Error thrown handling request: some error")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
