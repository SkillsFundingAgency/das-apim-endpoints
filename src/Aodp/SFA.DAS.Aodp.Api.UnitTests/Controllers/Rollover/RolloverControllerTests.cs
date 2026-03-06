using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Api.Controllers.Rollover;
using SFA.DAS.Aodp.Api.UnitTests.Controllers.Qualification;
using SFA.DAS.Aodp.Application.Queries.Rollover;

namespace SFA.DAS.Aodp.Api.UnitTests.Controllers.Rollover;

[TestFixture]
public class RolloverControllerTests
{
    private IFixture _fixture;
    private Mock<ILogger<RolloverController>> _mockLogger;
    private Mock<IMediator> _mockMediator;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _fixture.Customizations.Add(new DateOnlySpecimenBuilder());

        _mockLogger = _fixture.Freeze<Mock<ILogger<RolloverController>>>();
        _mockMediator = _fixture.Freeze<Mock<IMediator>>();
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


    [Test]
    public async Task GetRolloverCandidates_ReturnsOk_WithCandidatesList()
    {
        // Arrange
        var response = _fixture.Create<BaseMediatrResponse<GetRolloverCandidatesQueryResponse>>();
        response.Success = true;
        response.Value.RolloverCandidates = _fixture.CreateMany<RolloverCandidate>(3).ToList();

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        _mockMediator.Setup(m => m.Send(It.IsAny<GetRolloverCandidatesQuery>(), default))
                     .ReturnsAsync(response);

        // Act
        var result = await controller.GetRolloverCandidates();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;

        Assert.That(okResult.Value, Is.AssignableFrom<GetRolloverCandidatesQueryResponse>());

        var model = (GetRolloverCandidatesQueryResponse)okResult.Value;
        Assert.That(model.RolloverCandidates.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task GetRolloverCandidates_ReturnsInternalServerError_WhenSuccessIsFalse()
    {
        // Arrange
        var response = new BaseMediatrResponse<GetRolloverCandidatesQueryResponse>
        {
            Success = false,
            ErrorMessage = "some error"
        };

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetRolloverCandidatesQuery>(), default))
            .ReturnsAsync(response);

        // Act
        var result = await controller.GetRolloverCandidates();

        // Assert
        const int expectedStatus = StatusCodes.Status500InternalServerError;

        Assert.That(result, Is.InstanceOf<StatusCodeResult>());
        var status = (StatusCodeResult)result;
        Assert.That(status.StatusCode, Is.EqualTo(expectedStatus));
    }

    [Test]
    public async Task GetRolloverCandidates_WhenMediatorReturnsFailure_ShouldReturn500()
    {
        // Arrange
        var mediatrResponse = new BaseMediatrResponse<GetRolloverCandidatesQueryResponse>
        {
            Success = false,
            ErrorMessage = "some error"
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetRolloverCandidatesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatrResponse);

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        // Act
        var actionResult = await controller.GetRolloverCandidates();

        // Assert
        const int expectedStatus = StatusCodes.Status500InternalServerError;

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
            Assert.Fail(
                $"Expected ObjectResult or StatusCodeResult with status {expectedStatus}, " +
                $"but got {actionResult?.GetType().FullName ?? "null"}");
        }
    }
}
