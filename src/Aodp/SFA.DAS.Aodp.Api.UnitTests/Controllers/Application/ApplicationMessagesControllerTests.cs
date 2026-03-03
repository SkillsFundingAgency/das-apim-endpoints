using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Application.Commands.Application.Application;
using SFA.DAS.Aodp.Application.Queries.Application.Application;

namespace SFA.DAS.Aodp.Api.UnitTests.Controllers.Application;

[TestFixture]
public class ApplicationMessagesControllerTests
{
    private IFixture _fixture;
    private Mock<ILogger<Api.Controllers.Application.ApplicationMessagesController>> _loggerMock;
    private Mock<IMediator> _mediatorMock;
    private Api.Controllers.Application.ApplicationMessagesController _controller;


    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _loggerMock = _fixture.Freeze<Mock<ILogger<Api.Controllers.Application.ApplicationMessagesController>>>();
        _mediatorMock = _fixture.Freeze<Mock<IMediator>>();
        _controller = new Api.Controllers.Application.ApplicationMessagesController(_mediatorMock.Object, _loggerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _controller.Dispose();
    }

    [Test]
    public async Task GetMessageByIdAsync_ReturnsOkResult()
    {
        // Arrange
        var response = _fixture.Create<GetApplicationMessageByIdQueryResponse>();
        BaseMediatrResponse<GetApplicationMessageByIdQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetApplicationMessageByIdQuery>(), default))
            .ReturnsAsync(wrapper);

        var msgId = Guid.NewGuid();

        // Act
        var result = await _controller.GetMessageByIdAsync(msgId);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetApplicationMessageByIdQuery>(), default), Times.Once());
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetApplicationMessageByIdQueryResponse>());
        var model = (GetApplicationMessageByIdQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task BulkApplicationMessages_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<BulkApplicationMessageCommand>();
        var response = _fixture.Create<BulkApplicationMessageCommandResponse>();

        var wrapper = new BaseMediatrResponse<BulkApplicationMessageCommandResponse>
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<BulkApplicationMessageCommand>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.BulkApplicationMessages(request);

        // Assert
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<BulkApplicationMessageCommand>(c =>
                    c.ApplicationIds.SequenceEqual(request.ApplicationIds) &&
                    c.ShareWithOfqual == request.ShareWithOfqual &&
                    c.ShareWithSkillsEngland == request.ShareWithSkillsEngland &&
                    c.Unlock == request.Unlock &&
                    c.SentByName == request.SentByName &&
                    c.SentByEmail == request.SentByEmail
                ),
                default),
            Times.Once);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));

        var model = okResult.Value as BulkApplicationMessageCommandResponse;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task BulkApplicationMessages_WhenMediatorFails_ReturnsInternalServerError()
    {
        // Arrange
        var request = _fixture.Create<BulkApplicationMessageCommand>();

        var wrapper = new BaseMediatrResponse<BulkApplicationMessageCommandResponse>
        {
            Success = false,
            ErrorMessage = "Some error"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<BulkApplicationMessageCommand>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.BulkApplicationMessages(request);

        // Assert
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<BulkApplicationMessageCommand>(c =>
                    c.ApplicationIds.SequenceEqual(request.ApplicationIds)
                ),
                default),
            Times.Once);

        Assert.That(result, Is.InstanceOf<StatusCodeResult>());
        var status = (StatusCodeResult)result;
        Assert.That(status.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }



}
