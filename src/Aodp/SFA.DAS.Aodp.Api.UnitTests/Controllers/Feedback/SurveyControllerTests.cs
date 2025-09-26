using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Api.Controllers.Feedback;
using SFA.DAS.Aodp.Application.Commands.Feedback;

namespace SFA.DAS.Aodp.Api.UnitTests.Controllers.Feedback
{
    [TestFixture]
    public class SurveyControllerTests
    {
        private IFixture _fixture;
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<SurveyController>> _loggerMock;
        private SurveyController _controller;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mediatorMock = _fixture.Freeze<Mock<IMediator>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<SurveyController>>>();
            _controller = new SurveyController(_mediatorMock.Object, _loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [Test]
        public async Task SaveSurveyAsync_ReturnsOkResult_WhenRequestIsSuccessful()
        {
            // Arrange
            var command = _fixture.Create<SaveSurveyCommand>();
            var response = new BaseMediatrResponse<EmptyResponse> { Success = true };
            _mediatorMock.Setup(m => m.Send(It.IsAny<SaveSurveyCommand>(), default)).ReturnsAsync(response);

            // Act
            var result = await _controller.SaveSurveyAsync(command);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<SaveSurveyCommand>(), default), Times.Once());
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.AssignableFrom<EmptyResponse>());
        }

        [Test]
        public async Task SaveSurveyAsync_ReturnsInternalServerError_WhenRequestFails()
        {
            // Arrange
            var command = _fixture.Create<SaveSurveyCommand>();
            var response = new BaseMediatrResponse<EmptyResponse> { Success = false, ErrorMessage = "Error" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<SaveSurveyCommand>(), default)).ReturnsAsync(response);

            // Act
            var result = await _controller.SaveSurveyAsync(command);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<SaveSurveyCommand>(), default), Times.Once());
            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusCodeResult = (StatusCodeResult)result;
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }
    }
}
