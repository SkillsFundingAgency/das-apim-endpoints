using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Api.Controllers;
using SFA.DAS.Aodp.Api.Controllers.FormBuilder;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Forms;

namespace SFA.DAS.Aodp.Api.UnitTests.Controllers.FormBuilder;

[TestFixture]
public class FormsControllerTests
{
    private IFixture _fixture;
    private Mock<ILogger<FormsController>> _loggerMock;
    private Mock<IMediator> _mediatorMock;
    private FormsController _controller;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _loggerMock = _fixture.Freeze<Mock<ILogger<FormsController>>>();
        _mediatorMock = _fixture.Freeze<Mock<IMediator>>();
        _controller = new FormsController(_mediatorMock.Object, _loggerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _controller.Dispose();
    }

    [Test]
    public async Task GetAllAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<GetAllFormVersionsQuery>();
        var response = _fixture.Create<GetAllFormVersionsQueryResponse>();
        BaseMediatrResponse<GetAllFormVersionsQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllFormVersionsQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetAllAsync();

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Never());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetAllFormVersionsQueryResponse>());
        var model = (GetAllFormVersionsQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task GetByIdAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<GetFormVersionByIdQuery>();
        var response = _fixture.Create<GetFormVersionByIdQueryResponse>();
        BaseMediatrResponse<GetFormVersionByIdQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetFormVersionByIdQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetByIdAsync(request.FormVersionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Never());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetFormVersionByIdQueryResponse>());
        var model = (GetFormVersionByIdQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task CreateAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<CreateFormVersionCommand>();
        var response = _fixture.Create<CreateFormVersionCommandResponse>();
        BaseMediatrResponse<CreateFormVersionCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateFormVersionCommand>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.CreateAsync(request);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<CreateFormVersionCommandResponse>());
        var model = (CreateFormVersionCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task UpdateAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<UpdateFormVersionCommand>();
        var response = _fixture.Create<UpdateFormVersionCommandResponse>();
        BaseMediatrResponse<UpdateFormVersionCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateFormVersionCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.UpdateAsync(request.FormVersionId, request);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<UpdateFormVersionCommandResponse>());
        var model = (UpdateFormVersionCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task PublishAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<PublishFormVersionCommand>();
        var response = _fixture.Create<PublishFormVersionCommandResponse>();
        BaseMediatrResponse<PublishFormVersionCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<PublishFormVersionCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.PublishAsync(request.FormVersionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Never());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<PublishFormVersionCommandResponse>());
        var model = (PublishFormVersionCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task UnpublishAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<UnpublishFormVersionCommand>();
        var response = _fixture.Create<UnpublishFormVersionCommandResponse>();
        BaseMediatrResponse<UnpublishFormVersionCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UnpublishFormVersionCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.UnpublishAsync(request.FormVersionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Never());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<UnpublishFormVersionCommandResponse>());
        var model = (UnpublishFormVersionCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task MoveUpAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<MoveFormUpCommand>();
        var response = _fixture.Create<MoveFormUpCommandResponse>();
        BaseMediatrResponse<MoveFormUpCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<MoveFormUpCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.MoveUpAsync(request.FormId);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Never());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<MoveFormUpCommandResponse>());
        var model = (MoveFormUpCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task MoveDownAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<MoveFormDownCommand>();
        var response = _fixture.Create<MoveFormDownCommandResponse>();
        BaseMediatrResponse<MoveFormDownCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<MoveFormDownCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.MoveDownAsync(request.FormId);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Never());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<MoveFormDownCommandResponse>());
        var model = (MoveFormDownCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task CreateDraftAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<CreateDraftFormVersionCommand>();
        var response = _fixture.Create<CreateDraftFormVersionCommandResponse>();
        BaseMediatrResponse<CreateDraftFormVersionCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateDraftFormVersionCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.CreateDraftAsync(request.FormId);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Never());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<CreateDraftFormVersionCommandResponse>());
        var model = (CreateDraftFormVersionCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task RemoveAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<DeleteFormVersionCommand>();
        var response = _fixture.Create<DeleteFormVersionCommandResponse>();
        BaseMediatrResponse<DeleteFormVersionCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteFormVersionCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.RemoveAsync(request.FormVersionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Never());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<DeleteFormVersionCommandResponse>());
        var model = (DeleteFormVersionCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }
}