using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Api.Controllers;
using SFA.DAS.Aodp.Api.Controllers.FormBuilder;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Pages;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Questions;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Pages;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Questions;

namespace SFA.DAS.Aodp.Api.UnitTests.Controllers.FormBuilder;

[TestFixture]
public class QuestionsControllerTests
{
    private IFixture _fixture;
    private Mock<ILogger<QuestionsController>> _loggerMock;
    private Mock<IMediator> _mediatorMock;
    private QuestionsController _controller;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _loggerMock = _fixture.Freeze<Mock<ILogger<QuestionsController>>>();
        _mediatorMock = _fixture.Freeze<Mock<IMediator>>();
        _controller = new QuestionsController(_mediatorMock.Object, _loggerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _controller.Dispose();
    }

    [Test]
    public async Task GetByIdAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<GetQuestionByIdQuery>();
        var response = new GetQuestionByIdQueryResponse()
        {
            Id = Guid.NewGuid(),
            PageId = Guid.NewGuid(),
            Title = "",
            Key = Guid.NewGuid(),
            Hint = "",
            Order = 0,
            Required = false,
            Type = "",
            TextInput = new(),
            NumberInput = new(),
            Checkbox = new(),
            DateInput = new(),
            FileUpload = new(),
            Options = new(),
            Routes = new(),
            Editable = true
        };
        BaseMediatrResponse<GetQuestionByIdQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetQuestionByIdQuery>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.GetByIdAsync(request.FormVersionId, request.PageId, request.SectionId, request.QuestionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetQuestionByIdQuery>(), default), Times.Once());
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<GetQuestionByIdQuery>(q =>
                    q.FormVersionId == request.FormVersionId
                    && q.SectionId == request.SectionId
                    && q.PageId == request.PageId
                    && q.QuestionId == request.QuestionId
        ), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetQuestionByIdQueryResponse>());
        var model = (GetQuestionByIdQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task CreateAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<CreateQuestionCommand>();
        var response = _fixture.Create<CreateQuestionCommandResponse>();
        BaseMediatrResponse<CreateQuestionCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateQuestionCommand>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.CreateAsync(request.FormVersionId, request.SectionId, request.PageId, request);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<CreateQuestionCommandResponse>());
        var model = (CreateQuestionCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task UpdateAsync_ReturnsOkResult()
    {
        // Arrange
        var request = new UpdateQuestionCommand()
        {
            Id = Guid.NewGuid(),
            FormVersionId = Guid.NewGuid(),
            SectionId = Guid.NewGuid(),

            PageId = Guid.NewGuid(),
            Title = "",
            Hint = "",
            Required = false
        };
        var response = _fixture.Create<EmptyResponse>();
        BaseMediatrResponse<EmptyResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateQuestionCommand>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.UpdateAsync(request.FormVersionId, request.SectionId, request.PageId, request.Id, request);

        // Assert
        _mediatorMock.Verify(m => m.Send(request, default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<EmptyResponse>());
        var model = (EmptyResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task MoveUpAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<MoveQuestionUpCommand>();
        var response = _fixture.Create<MoveQuestionUpCommandResponse>();
        BaseMediatrResponse<MoveQuestionUpCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<MoveQuestionUpCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.MoveUpAsync(request.FormVersionId, request.SectionId, request.PageId, request.QuestionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<MoveQuestionUpCommand>(), default), Times.Once());
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<MoveQuestionUpCommand>(q =>
                    q.FormVersionId == request.FormVersionId
                    && q.SectionId == request.SectionId
                    && q.PageId == request.PageId
                    && q.QuestionId == request.QuestionId
        ), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<MoveQuestionUpCommandResponse>());
        var model = (MoveQuestionUpCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task MoveDownAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<MoveQuestionDownCommand>();
        var response = _fixture.Create<MoveQuestionDownCommandResponse>();
        BaseMediatrResponse<MoveQuestionDownCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<MoveQuestionDownCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.MoveDownAsync(request.FormVersionId, request.SectionId, request.PageId, request.QuestionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<MoveQuestionDownCommand>(), default), Times.Once());
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<MoveQuestionDownCommand>(q =>
                    q.FormVersionId == request.FormVersionId
                    && q.SectionId == request.SectionId
                    && q.PageId == request.PageId
                    && q.QuestionId == request.QuestionId
        ), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<MoveQuestionDownCommandResponse>());
        var model = (MoveQuestionDownCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task RemoveAsync_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<DeleteQuestionCommand>();
        var response = _fixture.Create<DeleteQuestionCommandResponse>();
        BaseMediatrResponse<DeleteQuestionCommandResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteQuestionCommand>(), default))
            .Returns(Task.FromResult(wrapper));

        // Act
        var result = await _controller.DeleteByIdAsync(request.FormVersionId, request.SectionId, request.PageId, request.QuestionId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteQuestionCommand>(), default), Times.Once());
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<DeleteQuestionCommand>(q =>
                    q.FormVersionId == request.FormVersionId
                    && q.SectionId == request.SectionId
                    && q.PageId == request.PageId
                    && q.QuestionId == request.QuestionId
        ), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<DeleteQuestionCommandResponse>());
        var model = (DeleteQuestionCommandResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }
}