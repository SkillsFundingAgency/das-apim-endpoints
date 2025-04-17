using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
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

}
