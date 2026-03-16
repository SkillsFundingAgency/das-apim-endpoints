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
        var payload = new BaseMediatrResponse<GetRolloverWorkflowCandidatesCountQueryResponse>
        {
            Value = new GetRolloverWorkflowCandidatesCountQueryResponse
            {
                TotalRecords = 5
            },
        };
        var mediatrResponse = new BaseMediatrResponse<GetRolloverWorkflowCandidatesCountQueryResponse>
        {
            Success = true,
            Value = payload.Value
        };

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetRolloverWorkflowCandidatesCountQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatrResponse);

        // Act
        var actionResult = await controller.GetRolloverWorkflowCandidatesCount(default);

        // Assert
        Assert.That(actionResult, Is.InstanceOf<OkObjectResult>());
        var ok = (OkObjectResult)actionResult;
        Assert.That(ok.StatusCode, Is.EqualTo(200));
        Assert.That(ok.Value, Is.InstanceOf<BaseMediatrResponse<GetRolloverWorkflowCandidatesCountQueryResponse>>());
        var returned = (BaseMediatrResponse<GetRolloverWorkflowCandidatesCountQueryResponse>)ok.Value;
        Assert.That(returned.Success, Is.EqualTo(true));
        Assert.That(returned.Value, Is.EqualTo(payload.Value));

        _mockMediator.Verify(m => m.Send(It.IsAny<GetRolloverWorkflowCandidatesCountQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task GetRolloverWorkflowCandidates_WhenMediatorReturnsFailure_ShouldReturnErrorMessage()
    {
        // Arrange
        var mediatrResponse = new BaseMediatrResponse<GetRolloverWorkflowCandidatesCountQueryResponse>
        {
            Success = false,
            ErrorMessage = "some error"
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetRolloverWorkflowCandidatesCountQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatrResponse);
        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        // Act
        var actionResult = await controller.GetRolloverWorkflowCandidatesCount(default);

        // Assert
        Assert.That(actionResult, Is.InstanceOf<OkObjectResult>());
        var status = (OkObjectResult)actionResult;

        Assert.That(status.Value, Is.InstanceOf<BaseMediatrResponse<GetRolloverWorkflowCandidatesCountQueryResponse>>());
        var value = (BaseMediatrResponse<GetRolloverWorkflowCandidatesCountQueryResponse>)status.Value;

        Assert.That(value.Success, Is.False);
        Assert.That(value.ErrorMessage, Is.EqualTo("some error"));
    }
}
