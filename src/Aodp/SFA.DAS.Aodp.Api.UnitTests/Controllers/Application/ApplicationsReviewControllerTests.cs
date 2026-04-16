using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Application.Commands.Application.Application;
using SFA.DAS.Aodp.Application.Queries.Application.Review;

namespace SFA.DAS.Aodp.Api.UnitTests.Controllers.Application;

[TestFixture]
public class ApplicationsReviewControllerTests
{
    private IFixture _fixture;
    private Mock<ILogger<Api.Controllers.Application.ApplicationsReviewController>> _loggerMock;
    private Mock<IMediator> _mediatorMock;
    private Api.Controllers.Application.ApplicationsReviewController _controller;


    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _loggerMock = _fixture.Freeze<Mock<ILogger<Api.Controllers.Application.ApplicationsReviewController>>>();
        _mediatorMock = _fixture.Freeze<Mock<IMediator>>();
        _controller = new Api.Controllers.Application.ApplicationsReviewController(_mediatorMock.Object, _loggerMock.Object);
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
        var response = new GetQfauFeedbackForApplicationReviewConfirmationQueryResponse();
        BaseMediatrResponse<GetQfauFeedbackForApplicationReviewConfirmationQueryResponse> wrapper = new()
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetQfauFeedbackForApplicationReviewConfirmationQuery>(), default))
            .ReturnsAsync(wrapper);

        var id = Guid.NewGuid();

        // Act
        var result = await _controller.GetQfauFeedbackForApplicationReviewConfirmationById(id);

        // Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetQfauFeedbackForApplicationReviewConfirmationQuery>(), default), Times.Once());
        _mediatorMock.Verify(m => m.Send(It.Is<GetQfauFeedbackForApplicationReviewConfirmationQuery>(a => a.ApplicationReviewId == id), default), Times.Once());

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.AssignableFrom<GetQfauFeedbackForApplicationReviewConfirmationQueryResponse>());
        var model = (GetQfauFeedbackForApplicationReviewConfirmationQueryResponse)okResult.Value;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task BulkApplicationAction_ReturnsOkResult()
    {
        // Arrange
        var request = _fixture.Create<BulkApplicationActionCommand>();
        var response = _fixture.Create<BulkApplicationActionCommandResponse>();

        var wrapper = new BaseMediatrResponse<BulkApplicationActionCommandResponse>
        {
            Value = response,
            Success = true
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<BulkApplicationActionCommand>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.BulkApplicationAction(request);

        // Assert
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<BulkApplicationActionCommand>(c =>
                    c.ApplicationReviewIds.SequenceEqual(request.ApplicationReviewIds) &&
                    c.ActionType == request.ActionType &&
                    c.SentByName == request.SentByName &&
                    c.SentByEmail == request.SentByEmail
                ),
                default),
            Times.Once);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));

        var model = okResult.Value as BulkApplicationActionCommandResponse;
        Assert.That(model, Is.EqualTo(response));
    }

    [Test]
    public async Task BulkApplicationAction_WhenMediatorFails_ReturnsInternalServerError()
    {
        // Arrange
        var request = _fixture.Create<BulkApplicationActionCommand>();

        var wrapper = new BaseMediatrResponse<BulkApplicationActionCommandResponse>
        {
            Success = false,
            ErrorMessage = "Some error"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<BulkApplicationActionCommand>(), default))
            .ReturnsAsync(wrapper);

        // Act
        var result = await _controller.BulkApplicationAction(request);

        // Assert
        _mediatorMock.Verify(m =>
            m.Send(
                It.Is<BulkApplicationActionCommand>(c =>
                    c.ApplicationReviewIds.SequenceEqual(request.ApplicationReviewIds)
                ),
                default),
            Times.Once);

        Assert.That(result, Is.InstanceOf<StatusCodeResult>());
        var status = (StatusCodeResult)result;
        Assert.That(status.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

}
