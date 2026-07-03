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
using SFA.DAS.AODP.Application.Commands.Rollover;

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
        var payload = new GetRolloverWorkflowCandidatesCountQueryResponse
        {
            TotalRecords = 5
        };
        var mediatrResponse = new BaseMediatrResponse<GetRolloverWorkflowCandidatesCountQueryResponse>
        {
            Success = true,
            Value = payload
        };

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetRolloverWorkflowCandidatesCountQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatrResponse);

        // Act
        var actionResult = await controller.GetRolloverWorkflowCandidatesCount(CancellationToken.None);

        // Assert
        Assert.That(actionResult, Is.InstanceOf<OkObjectResult>());
        var ok = (OkObjectResult)actionResult;
        Assert.That(ok.StatusCode, Is.EqualTo(200));
        Assert.That(ok.Value, Is.InstanceOf<GetRolloverWorkflowCandidatesCountQueryResponse>());
        var returned = (GetRolloverWorkflowCandidatesCountQueryResponse)ok.Value;

        Assert.That(returned, Is.EqualTo(payload));

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
        var actionResult = await controller.GetRolloverWorkflowCandidatesCount(CancellationToken.None);

        // Assert
        Assert.That(actionResult, Is.InstanceOf<StatusCodeResult>());
        var status = (StatusCodeResult)actionResult;
        Assert.AreEqual(500, status.StatusCode);
    }


    [Test]
    public async Task GetRolloverCandidates_ReturnsOk_WithCandidatesList()
    {
        // Arrange
        var response = _fixture.Create<BaseMediatrResponse<GetRolloverCandidatesQueryResponse>>();
        response.Success = true;
        response.Value.RolloverCandidates = _fixture.CreateMany<RolloverCandidate>(3).ToList();

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        _mockMediator.Setup(m => m.Send(It.IsAny<GetRolloverCandidatesQuery>(), CancellationToken.None))
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
            .Setup(m => m.Send(It.IsAny<GetRolloverCandidatesQuery>(), CancellationToken.None))
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

    [Test]
    public async Task GetRolloverCandidatesForExport_WhenMediatorReturnsSuccess_ShouldReturnOkWithValue()
    {
        // Arrange
        var workflowRunId = Guid.NewGuid();

        var expected = new GetRolloverCandidatesForExportQueryResponse
        {
            FileContent = new byte[] { 1, 2, 3 },
            FileName = "export.csv",
            ContentType = "text/csv"
        };

        var mediatorResponse = new BaseMediatrResponse<GetRolloverCandidatesForExportQueryResponse>
        {
            Success = true,
            Value = expected
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetRolloverCandidatesForExportQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        // Act
        var result = await controller.GetRolloverCandidatesForExport(workflowRunId);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var ok = (OkObjectResult)result;

        Assert.That(ok.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That(ok.Value, Is.InstanceOf<GetRolloverCandidatesForExportQueryResponse>());

        var returned = (GetRolloverCandidatesForExportQueryResponse)ok.Value!;
        Assert.That(returned.FileName, Is.EqualTo("export.csv"));
        Assert.That(returned.ContentType, Is.EqualTo("text/csv"));
        Assert.That(returned.FileContent, Is.EqualTo(new byte[] { 1, 2, 3 }));

        _mockMediator.Verify(m => m.Send(
            It.IsAny<GetRolloverCandidatesForExportQuery>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task GetRolloverCandidatesForExport_WhenMediatorReturnsFailure_ShouldReturn500()
    {
        // Arrange
        var workflowRunId = Guid.NewGuid();

        var mediatorResponse = new BaseMediatrResponse<GetRolloverCandidatesForExportQueryResponse>
        {
            Success = false,
            ErrorMessage = "something went wrong"
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetRolloverCandidatesForExportQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        // Act
        var result = await controller.GetRolloverCandidatesForExport(workflowRunId);

        // Assert
        const int expectedStatus = StatusCodes.Status500InternalServerError;

        if (result is ObjectResult objectResult)
            Assert.That(objectResult.StatusCode, Is.EqualTo(expectedStatus));
        else if (result is StatusCodeResult statusCodeResult)
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(expectedStatus));
        else
            Assert.Fail($"Expected 500 result, got {result?.GetType().FullName ?? "null"}");
    }

    [Test]
    public async Task ValidateRolloverExtension_WhenMediatorReturnsSuccess_ShouldReturnOkWithValue()
    {
        // Arrange
        var command = new ValidateRolloverExtensionCommand
        {
            RolloverCandidates =
            [
                new() { Qan = "123", FundingStreamName = "FS", RollOverStatus = "ToExtend", ProposedFundingApprovalEndDate = DateTime.UtcNow }
            ]
        };

        var mediatorResponse = new BaseMediatrResponse<ValidateRolloverExtensionCommandResponse>
        {
            Success = true,
            Value = new ValidateRolloverExtensionCommandResponse
            {
                IsValid = true,
                ValidationSuccessSummary = new FundingExtensionSummary
                {
                    TotalCandidatesCount = 10,
                    CandidatesExtendedInUploadCount = 2,
                    TotalCandidatesToBeExtendedCount = 5,
                    TotalCandidatesToBeExcludedCount = 1,
                    TotalCandidatesToBeReviewedCount = 4
                }
            }
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<ValidateRolloverExtensionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        // Act
        var result = await controller.ValidateRolloverExtension(command);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var ok = (OkObjectResult)result;

        Assert.That(ok.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That(ok.Value, Is.InstanceOf<ValidateRolloverExtensionCommandResponse>());

        var returned = (ValidateRolloverExtensionCommandResponse)ok.Value!;
        Assert.That(returned.IsValid, Is.True);

        _mockMediator.Verify(m => m.Send(
            It.IsAny<ValidateRolloverExtensionCommand>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }


    [Test]
    public async Task ValidateRolloverExtension_WhenMediatorReturnsFailure_ShouldReturn500()
    {
        // Arrange
        var command = new ValidateRolloverExtensionCommand
        {
            RolloverCandidates =
            [
                new() { Qan = "123", FundingStreamName = "FS", RollOverStatus = "ToExtend" , ProposedFundingApprovalEndDate =  DateTime.UtcNow}
            ]
        };

        var mediatorResponse = new BaseMediatrResponse<ValidateRolloverExtensionCommandResponse>
        {
            Success = false,
            ErrorMessage = "Validation failed"
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<ValidateRolloverExtensionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        // Act
        var result = await controller.ValidateRolloverExtension(command);

        // Assert
        const int expectedStatus = StatusCodes.Status500InternalServerError;

        if (result is ObjectResult objectResult)
            Assert.That(objectResult.StatusCode, Is.EqualTo(expectedStatus));
        else if (result is StatusCodeResult statusCodeResult)
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(expectedStatus));
        else
            Assert.Fail($"Expected 500 result, got {result?.GetType().FullName ?? "null"}");

        _mockMediator.Verify(m => m.Send(
            It.IsAny<ValidateRolloverExtensionCommand>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task ValidateRolloverExtension_ShouldSendCommandToMediator()
    {
        // Arrange
        var command = new ValidateRolloverExtensionCommand
        {
            RolloverCandidates =
            [
                new() { Qan = "123", FundingStreamName = "FS", RollOverStatus = "ToExtend", ProposedFundingApprovalEndDate = DateTime.UtcNow }
            ]
        };

        ValidateRolloverExtensionCommand? captured = null;

        _mockMediator
            .Setup(m => m.Send(It.IsAny<ValidateRolloverExtensionCommand>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<BaseMediatrResponse<ValidateRolloverExtensionCommandResponse>>, CancellationToken>((cmd, _) =>
            {
                captured = cmd as ValidateRolloverExtensionCommand;
            })
            .ReturnsAsync(new BaseMediatrResponse<ValidateRolloverExtensionCommandResponse> { Success = true });

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        // Act
        await controller.ValidateRolloverExtension(command);

        // Assert
        Assert.That(captured, Is.Not.Null);
        Assert.That(captured, Is.EqualTo(command));
    }

    [Test]
    public async Task SubmitRolloverExtension_WhenMediatorReturnsSuccess_ShouldReturnOkWithValue()
    {
        // Arrange
        var command = new SubmitRolloverExtensionCommand
        {
            Items =
            [
                new() 
                { 
                    Qan = "123", 
                    FundingStreamName = "FS", 
                    RolloverStatus = "Extended", 
                    ProposedFundingApprovalEndDate = DateTime.UtcNow.AddMonths(3)
                }
            ]
        };

        var returnMessageText = "The return value";

        var mediatorResponse = new BaseMediatrResponse<SubmitRolloverExtensionCommandResponse>
        {
            Success = true,
            Value = new SubmitRolloverExtensionCommandResponse
            {
                ResultMessage = returnMessageText
            }
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<SubmitRolloverExtensionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        // Act
        var result = await controller.SubmitRolloverExtension(command);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var ok = (OkObjectResult)result;

        Assert.That(ok.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That(ok.Value, Is.InstanceOf<SubmitRolloverExtensionCommandResponse>());

        var returned = (SubmitRolloverExtensionCommandResponse)ok.Value!;
        Assert.That(returned.ResultMessage, Is.EqualTo(returnMessageText));

        _mockMediator.Verify(m => m.Send(
            It.IsAny<SubmitRolloverExtensionCommand>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task SubmitRolloverExtension_WhenMediatorReturnsFailure_ShouldReturn500()
    {
        // Arrange
        var command = new SubmitRolloverExtensionCommand
        {
            Items =
            [
                new FundingExtensionItem
                {
                    Qan = "123",
                    FundingStreamName = "FS",
                    RolloverStatus = "Extended",
                    ProposedFundingApprovalEndDate = DateTime.UtcNow
                }
            ]
        };

        var mediatorResponse = new BaseMediatrResponse<SubmitRolloverExtensionCommandResponse>
        {
            Success = false,
            ErrorMessage = "Submit failed"
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<SubmitRolloverExtensionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResponse);

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        // Act
        var result = await controller.SubmitRolloverExtension(command);

        // Assert
        const int expectedStatus = StatusCodes.Status500InternalServerError;

        if (result is ObjectResult objectResult)
            Assert.That(objectResult.StatusCode, Is.EqualTo(expectedStatus));
        else if (result is StatusCodeResult statusCodeResult)
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(expectedStatus));
        else
            Assert.Fail($"Expected 500 result, got {result?.GetType().FullName ?? "null"}");

        _mockMediator.Verify(m => m.Send(
            It.IsAny<SubmitRolloverExtensionCommand>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task SubmitRolloverExtension_ShouldSendCommandToMediator()
    {
        // Arrange
        var command = new SubmitRolloverExtensionCommand
        {
            Items =
            [
                new FundingExtensionItem
            {
                Qan = "123",
                FundingStreamName = "FS",
                RolloverStatus = "Extended",
                ProposedFundingApprovalEndDate = DateTime.UtcNow
            }
            ]
        };

        SubmitRolloverExtensionCommand? captured = null;

        _mockMediator
            .Setup(m => m.Send(It.IsAny<SubmitRolloverExtensionCommand>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<BaseMediatrResponse<SubmitRolloverExtensionCommandResponse>>, CancellationToken>((cmd, _) =>
            {
                captured = cmd as SubmitRolloverExtensionCommand;
            })
            .ReturnsAsync(new BaseMediatrResponse<SubmitRolloverExtensionCommandResponse> { Success = true });

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        // Act
        await controller.SubmitRolloverExtension(command);

        // Assert
        Assert.That(captured, Is.Not.Null);
        Assert.That(captured, Is.EqualTo(command));
    }

    [Test]
    public async Task RemovePreviousWorkflowCandidates_WhenMediatorReturnsSuccess_ShouldReturnOkWithValue()
    {
        // Arrange
        var payload = new RemovePreviousWorkflowCandidatesCommandResponse
        {
            Success = true
        };
        var mediatrResponse = new BaseMediatrResponse<RemovePreviousWorkflowCandidatesCommandResponse>
        {
            Success = true,
            Value = payload
        };

        var command = new RemovePreviousWorkflowCandidatesCommand
        {
            RolloverWorkflowRunId = Guid.NewGuid(),
            CandidateIds = [Guid.NewGuid(), Guid.NewGuid()]
        };

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        _mockMediator
            .Setup(m => m.Send(It.IsAny<RemovePreviousWorkflowCandidatesCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatrResponse);

        // Act
        var actionResult = await controller.RemovePreviousWorkflowCandidates(command);

        // Assert
        Assert.That(actionResult, Is.InstanceOf<OkObjectResult>());
        var ok = (OkObjectResult)actionResult;
        Assert.That(ok.StatusCode, Is.EqualTo(200));
        Assert.That(ok.Value, Is.InstanceOf<RemovePreviousWorkflowCandidatesCommandResponse>());
        var returned = (RemovePreviousWorkflowCandidatesCommandResponse)ok.Value;

        Assert.That(returned.Success, Is.True);

        _mockMediator.Verify(m => m.Send(It.IsAny<RemovePreviousWorkflowCandidatesCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task RemovePreviousWorkflowCandidates_WhenMediatorReturnsFailure_ShouldReturn500Error()
    {
        // Arrange
        var mediatrResponse = new BaseMediatrResponse<RemovePreviousWorkflowCandidatesCommandResponse>
        {
            Success = false,
            ErrorMessage = "Failed to remove candidates"
        };

        var command = new RemovePreviousWorkflowCandidatesCommand
        {
            RolloverWorkflowRunId = Guid.NewGuid(),
            CandidateIds = [Guid.NewGuid()]
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<RemovePreviousWorkflowCandidatesCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatrResponse);

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        // Act
        var actionResult = await controller.RemovePreviousWorkflowCandidates(command);

        // Assert
        Assert.That(actionResult, Is.InstanceOf<StatusCodeResult>());
        var status = (StatusCodeResult)actionResult;
        Assert.That(status.StatusCode, Is.EqualTo(500));

        _mockMediator.Verify(m => m.Send(It.IsAny<RemovePreviousWorkflowCandidatesCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task RemovePreviousWorkflowCandidates_ShouldSendCommandToMediator()
    {
        // Arrange
        var rolloverWorkflowRunId = Guid.NewGuid();
        var candidateIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var command = new RemovePreviousWorkflowCandidatesCommand
        {
            RolloverWorkflowRunId = rolloverWorkflowRunId,
            CandidateIds = candidateIds
        };

        RemovePreviousWorkflowCandidatesCommand? captured = null;

        _mockMediator
            .Setup(m => m.Send(It.IsAny<RemovePreviousWorkflowCandidatesCommand>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<BaseMediatrResponse<RemovePreviousWorkflowCandidatesCommandResponse>>, CancellationToken>((cmd, _) =>
            {
                captured = cmd as RemovePreviousWorkflowCandidatesCommand;
            })
            .ReturnsAsync(new BaseMediatrResponse<RemovePreviousWorkflowCandidatesCommandResponse> 
            { 
                Success = true,
                Value = new RemovePreviousWorkflowCandidatesCommandResponse { Success = true }
            });

        var controller = new RolloverController(_mockMediator.Object, _mockLogger.Object);

        // Act
        await controller.RemovePreviousWorkflowCandidates(command);

        // Assert
        Assert.That(captured, Is.Not.Null);
        Assert.That(captured.RolloverWorkflowRunId, Is.EqualTo(rolloverWorkflowRunId));
        Assert.That(captured.CandidateIds, Is.EqualTo(candidateIds));
    }


}