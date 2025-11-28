using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Api.Controllers.Import;
using SFA.DAS.Aodp.Application.Commands.Import;
using System.Text;

namespace SFA.DAS.Aodp.Api.UnitTests.Controllers.Import;

[TestFixture]
public class ImportControllerTests
{
    private Mock<IMediator> _mediatorMock = null!;
    private Mock<ILogger<ImportController>> _loggerMock = null!;
    private ImportController? _controller = null;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<ImportController>>();
    }

    [Test]
    public async Task ImportDefundingList_WithSuccessfulResponse_ShouldReturnsOk()
    {
        // Arrange
        var file = CreateFormFile();
        var request = new ImportDefundingListCommand { File = file };

        ImportController _controller = new(_mediatorMock.Object, _loggerMock.Object);
        var expectedResponse = new BaseMediatrResponse<ImportDefundingListResponse>
        {
            Success = true,
            ErrorMessage = null,
            Value = new ImportDefundingListResponse { ImportedCount = 5, Message = "Imported" }
        };

        _mediatorMock
            .Setup(m => m.Send(
                It.Is<ImportDefundingListCommand>(cmd => ReferenceEquals(cmd.File, file)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.ImportDefundingList(request);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actionResult, Is.InstanceOf<OkObjectResult>());
            var ok = actionResult as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            Assert.That(ok!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));

            var actualValue = ok.Value as ImportDefundingListResponse;
            Assert.That(expectedResponse.Value.ImportedCount, Is.EqualTo(actualValue!.ImportedCount));
            Assert.That(expectedResponse.Value.Message, Is.EqualTo(actualValue.Message!));

            _mediatorMock.Verify(m => m.Send(
                It.Is<ImportDefundingListCommand>(cmd => ReferenceEquals(cmd.File, file)),
                It.IsAny<CancellationToken>()),
                Times.Once);
        });
    }

    [Test]
    public async Task ImportDefundingList_WhenMediatorThrows_ShouldReturnsInternalServerError()
    {
        // Arrange
        var file = CreateFormFile();
        var request = new ImportDefundingListCommand { File = file };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<IRequest<BaseMediatrResponse<ImportDefundingListResponse>>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("mediator failure"));

        ImportController _controller = new(_mediatorMock.Object, _loggerMock.Object);

        // Act
        var actionResult = await _controller.ImportDefundingList(request);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actionResult, Is.InstanceOf<ObjectResult>());
            var objResult = actionResult as ObjectResult;
            Assert.That(objResult, Is.Not.Null);
            Assert.That(objResult!.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));

            dynamic value = objResult.Value!;
            Assert.That(value, Is.Not.Null);
            var messageProp = value.GetType().GetProperty("message") ?? value.GetType().GetProperty("Message");
            Assert.That(messageProp, Is.Not.Null, "Response object does not contain a 'message' property");
            Assert.That((string)messageProp.GetValue(value), Is.EqualTo("Failed to read uploaded file"));

            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<BaseMediatrResponse<ImportDefundingListResponse>>>(), It.IsAny<CancellationToken>()), Times.Once);
        });
    }

    [Test]
    public async Task ImportPldns_WithSuccessfulResponse_ShouldReturnsOk()
    {
        // Arrange
        var file = CreateFormFile();
        var request = new ImportPldnsCommand { File = file };

        ImportController _controller = new(_mediatorMock.Object, _loggerMock.Object);
        var expectedResponse = new BaseMediatrResponse<ImportPldnsCommandResponse>
        {
            Success = true,
            ErrorMessage = null,
            Value = new ImportPldnsCommandResponse { ImportedCount = 5, Message = "Imported" }
        };

        _mediatorMock
            .Setup(m => m.Send(
                It.Is<ImportPldnsCommand>(cmd => ReferenceEquals(cmd.File, file)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var actionResult = await _controller.ImportPldns(request);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actionResult, Is.InstanceOf<OkObjectResult>());
            var ok = actionResult as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            Assert.That(ok!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));

            var actualValue = ok.Value as ImportPldnsCommandResponse;
            Assert.That(expectedResponse.Value.ImportedCount, Is.EqualTo(actualValue!.ImportedCount));
            Assert.That(expectedResponse.Value.Message, Is.EqualTo(actualValue.Message!));

            _mediatorMock.Verify(m => m.Send(
                It.Is<ImportPldnsCommand>(cmd => ReferenceEquals(cmd.File, file)),
                It.IsAny<CancellationToken>()),
                Times.Once);
        });
    }

    [Test]
    public async Task ImportPldns_WhenMediatorThrows_ShouldReturnsInternalServerError()
    {
        // Arrange
        var file = CreateFormFile();
        var request = new ImportPldnsCommand { File = file };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<IRequest<BaseMediatrResponse<ImportPldnsCommandResponse>>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("mediator failure"));

        ImportController _controller = new(_mediatorMock.Object, _loggerMock.Object);

        // Act
        var actionResult = await _controller.ImportPldns(request);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actionResult, Is.InstanceOf<ObjectResult>());
            var objResult = actionResult as ObjectResult;
            Assert.That(objResult, Is.Not.Null);
            Assert.That(objResult!.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));

            dynamic value = objResult.Value!;
            Assert.That(value, Is.Not.Null);
            var messageProp = value.GetType().GetProperty("message") ?? value.GetType().GetProperty("Message");
            Assert.That(messageProp, Is.Not.Null, "Response object does not contain a 'message' property");
            Assert.That((string)messageProp.GetValue(value), Is.EqualTo("Failed to read uploaded file"));

            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<BaseMediatrResponse<ImportPldnsCommandResponse>>>(), It.IsAny<CancellationToken>()), Times.Once);
        });
    }

    private static IFormFile CreateFormFile(string content = "col1,col2\r\n1,2")
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var ms = new MemoryStream(bytes);
        ms.Position = 0;
        var file = new FormFile(ms, 0, ms.Length, "file", "test.csv")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/csv"
        };
        return file;
    }
}