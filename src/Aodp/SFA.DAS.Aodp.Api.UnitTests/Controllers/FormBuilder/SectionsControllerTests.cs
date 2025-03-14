using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Api.Controllers;
using SFA.DAS.Aodp.Api.Controllers.FormBuilder;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Sections;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Routes;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Sections;

namespace SFA.DAS.Aodp.Api.UnitTests.Controllers.FormBuilder;

[TestFixture]
public class SectionsControllerTests
{
    private IFixture _fixture;
    private Mock<ILogger<SectionsController>> _loggerMock;
    private Mock<IMediator> _mediatorMock;
    private SectionsController _controller;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _loggerMock = _fixture.Freeze<Mock<ILogger<SectionsController>>>();
        _mediatorMock = _fixture.Freeze<Mock<IMediator>>();
        _controller = new SectionsController(_mediatorMock.Object, _loggerMock.Object);
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
        var request = _fixture.Create<GetAllSectionsQuery>();
        var response = _fixture.Create<GetAllSectionsQueryResponse>();
        BaseMediatrResponse<GetAllSectionsQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllSectionsQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetAllAsync(request.FormVersionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllSectionsQuery>(), default), Times.Once());
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<GetAllSectionsQuery>(q =>
                    q.FormVersionId == request.FormVersionId
        ), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetAllSectionsQueryResponse>());
        var model = (GetAllSectionsQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task GetByIdAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<GetSectionByIdQuery>();
        var response = _fixture.Create<GetSectionByIdQueryResponse>();
        BaseMediatrResponse<GetSectionByIdQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetSectionByIdQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetByIdAsync(request.SectionId, request.FormVersionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetSectionByIdQuery>(), default), Times.Once());
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<GetSectionByIdQuery>(q =>
                    q.SectionId == request.SectionId
                    && q.FormVersionId == request.FormVersionId
        ), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetSectionByIdQueryResponse>());
        var model = (GetSectionByIdQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task CreateAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<CreateSectionCommand>();
        var response = _fixture.Create<CreateSectionCommandResponse>();
        BaseMediatrResponse<CreateSectionCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateSectionCommand>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.CreateAsync(request.FormVersionId, request);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<CreateSectionCommandResponse>());
        var model = (CreateSectionCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task UpdateAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<UpdateSectionCommand>();
        var response = _fixture.Create<UpdateSectionCommandResponse>();
        BaseMediatrResponse<UpdateSectionCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateSectionCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.UpdateAsync(request.FormVersionId, request.Id, request);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<UpdateSectionCommandResponse>());
        var model = (UpdateSectionCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task MoveUpAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<MoveSectionUpCommand>();
        var response = _fixture.Create<MoveSectionUpCommandResponse>();
        BaseMediatrResponse<MoveSectionUpCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<MoveSectionUpCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.MoveUpAsync(request.FormVersionId, request.SectionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<MoveSectionUpCommand>(), default), Times.Once());
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<MoveSectionUpCommand>(q =>
                    q.SectionId == request.SectionId
                    && q.FormVersionId == request.FormVersionId
        ), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<MoveSectionUpCommandResponse>());
        var model = (MoveSectionUpCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task MoveDownAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<MoveSectionDownCommand>();
        var response = _fixture.Create<MoveSectionDownCommandResponse>();
        BaseMediatrResponse<MoveSectionDownCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<MoveSectionDownCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.MoveDownAsync(request.FormVersionId, request.SectionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<MoveSectionDownCommand>(), default), Times.Once());
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<MoveSectionDownCommand>(q =>
                    q.SectionId == request.SectionId
                    && q.FormVersionId == request.FormVersionId
        ), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<MoveSectionDownCommandResponse>());
        var model = (MoveSectionDownCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task RemoveAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<DeleteSectionCommand>();
        var response = _fixture.Create<DeleteSectionCommandResponse>();
        BaseMediatrResponse<DeleteSectionCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteSectionCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.RemoveAsync(request.SectionId, request.FormVersionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteSectionCommand>(), default), Times.Once());
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<DeleteSectionCommand>(q =>
                    q.SectionId == request.SectionId
                    && q.FormVersionId == request.FormVersionId
        ), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<DeleteSectionCommandResponse>());
        var model = (DeleteSectionCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }
}