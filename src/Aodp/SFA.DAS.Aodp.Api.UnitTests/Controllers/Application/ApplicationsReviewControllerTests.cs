using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
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
}
