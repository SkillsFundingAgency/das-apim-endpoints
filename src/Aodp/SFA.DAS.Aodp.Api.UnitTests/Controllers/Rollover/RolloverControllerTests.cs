using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Api.Controllers.Rollover;
using SFA.DAS.Aodp.Api.UnitTests.Controllers.Qualification;
using SFA.DAS.Aodp.Application.Commands.Rollover;
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

    [Test]
    public async Task CreateRolloverWorkflowRun_WhenMediatorReturnsSuccess_ShouldReturn200()
    {
        // Arrange
        var cmd = new CreateRolloverWorkflowRunCommand
        {
            AcademicYear = "2024/25",
            RolloverCandidateIds =  new List<Guid> { Guid.NewGuid() }
        };

        var createdId = Guid.NewGuid();
        var mediatorResponse = new BaseMediatrResponse<CreateRolloverWorkflowRunCommandResponse>
        {
            Success = true,
            Value = new CreateRolloverWorkflowRunCommandResponse { RolloverWorkflowRunId = createdId }
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<CreateRolloverWorkflowRunCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        // Act
        var result = await controller.CreateRolloverWorkflowRun(cmd);

        // Assert
        var ok = result as OkObjectResult;
        Assert.That(ok, Is.Not.Null);
        Assert.That(ok!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));

        // Expect the inner payload
        Assert.That(ok.Value, Is.InstanceOf<CreateRolloverWorkflowRunCommandResponse>());
        var payload = (CreateRolloverWorkflowRunCommandResponse)ok.Value!;
        Assert.That(payload.RolloverWorkflowRunId, Is.EqualTo(createdId));

    }

    [Test]
    public async Task CreateRolloverWorkflowRun_WhenMediatorReturnsFailure_ShouldReturn500()
    {
        // Arrange
        var cmd = new CreateRolloverWorkflowRunCommand
        {
            AcademicYear = "2024/25",
            RolloverCandidateIds = new List<Guid> { Guid.NewGuid() }
        };

        var mediatorResponse = new BaseMediatrResponse<CreateRolloverWorkflowRunCommandResponse>
        {
            Success = false,
            ErrorMessage = "Unexpected failure"
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<CreateRolloverWorkflowRunCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        // Act
        var result = await controller.CreateRolloverWorkflowRun(cmd);

        // Assert
        const int expected = StatusCodes.Status500InternalServerError;

        if (result is ObjectResult objectResult)
            Assert.That(objectResult.StatusCode, Is.EqualTo(expected));
        else if (result is StatusCodeResult statusCodeResult)
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(expected));
        else
            Assert.Fail($"Expected ObjectResult or StatusCodeResult with {expected}, got {result?.GetType().FullName ?? "null"}");
    }
}