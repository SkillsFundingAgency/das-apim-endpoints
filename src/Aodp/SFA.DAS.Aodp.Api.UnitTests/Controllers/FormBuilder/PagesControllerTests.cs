using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Api.Controllers;
using SFA.DAS.Aodp.Api.Controllers.FormBuilder;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Pages;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Forms;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Pages;

namespace SFA.DAS.Aodp.Api.UnitTests.Controllers.FormBuilder;

[TestFixture]
public class PagesControllerTests
{
    private IFixture _fixture;
    private Mock<ILogger<PagesController>> _loggerMock;
    private Mock<IMediator> _mediatorMock;
    private PagesController _controller;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _loggerMock = _fixture.Freeze<Mock<ILogger<PagesController>>>();
        _mediatorMock = _fixture.Freeze<Mock<IMediator>>();
        _controller = new PagesController(_mediatorMock.Object, _loggerMock.Object);
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
        var request = _fixture.Create<GetAllPagesQuery>();
        var response = _fixture.Create<GetAllPagesQueryResponse>();
        BaseMediatrResponse<GetAllPagesQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllPagesQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetAllAsync(request.SectionId, request.FormVersionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Never());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetAllPagesQueryResponse>());
        var model = (GetAllPagesQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task GetByIdAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<GetPageByIdQuery>();
        var response = _fixture.Create<GetPageByIdQueryResponse>();
        BaseMediatrResponse<GetPageByIdQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetPageByIdQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetByIdAsync(request.PageId, request.SectionId, request.FormVersionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Never());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetPageByIdQueryResponse>());
        var model = (GetPageByIdQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task GetPagePreviewByIdAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<GetPagePreviewByIdQuery>();
        var response = _fixture.Create<GetPagePreviewByIdQueryResponse>();
        BaseMediatrResponse<GetPagePreviewByIdQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetPagePreviewByIdQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetPagePreviewByIdAsync(request.FormVersionId, request.PageId, request.SectionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Never());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetPagePreviewByIdQueryResponse>());
        var model = (GetPagePreviewByIdQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task CreateAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<CreatePageCommand>();
        var response = _fixture.Create<CreatePageCommandResponse>();
        BaseMediatrResponse<CreatePageCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreatePageCommand>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.CreateAsync(request.FormVersionId, request.SectionId, request);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<CreatePageCommandResponse>());
        var model = (CreatePageCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task UpdateAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<UpdatePageCommand>();
        var response = _fixture.Create<UpdatePageCommandResponse>();
        BaseMediatrResponse<UpdatePageCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdatePageCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.UpdateAsync(request.FormVersionId, request.SectionId, request.Id, request);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<UpdatePageCommandResponse>());
        var model = (UpdatePageCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task MoveUpAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<MovePageUpCommand>();
        var response = _fixture.Create<MovePageUpCommandResponse>();
        BaseMediatrResponse<MovePageUpCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<MovePageUpCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.MoveUpAsync(request.FormVersionId, request.SectionId, request.PageId);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Never());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<MovePageUpCommandResponse>());
        var model = (MovePageUpCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task MoveDownAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<MovePageDownCommand>();
        var response = _fixture.Create<MovePageDownCommandResponse>();
        BaseMediatrResponse<MovePageDownCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<MovePageDownCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.MoveDownAsync(request.FormVersionId, request.SectionId, request.PageId);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Never());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<MovePageDownCommandResponse>());
        var model = (MovePageDownCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task RemoveAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<DeletePageCommand>();
        var response = _fixture.Create<DeletePageCommandResponse>();
        BaseMediatrResponse<DeletePageCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeletePageCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.RemoveAsync(request.PageId, request.FormVersionId, request.SectionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Never());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<DeletePageCommandResponse>());
        var model = (DeletePageCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }
}