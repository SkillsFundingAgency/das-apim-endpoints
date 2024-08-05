using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Earnings.Api.Controllers;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.Earnings.Api.UnitTests
{
    [TestFixture]
    public class ErrorControllerTests
    {
        private Mock<ILogger<ErrorController>> _loggerMock;
        private DefaultHttpContext _httpContext;
        private ErrorController _controller;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<ErrorController>>();
            _httpContext = new DefaultHttpContext();
            _controller = new ErrorController(_loggerMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _httpContext
                }
            };
        }

        [Test]
        public void Error_WhenApiUnauthorizedException_ShouldReturnUnauthorized()
        {
            // Arrange
            var exceptionFeatureMock = new Mock<IExceptionHandlerFeature>();
            exceptionFeatureMock.Setup(e => e.Error).Returns(new ApiUnauthorizedException());
            _httpContext.Features.Set(exceptionFeatureMock.Object);

            // Act
            var result = _controller.Error();

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Test]
        public void Error_WhenExceptionIsNull_ShouldThrowException()
        {
            // Arrange
            var exceptionFeatureMock = new Mock<IExceptionHandlerFeature>();
            exceptionFeatureMock.Setup(e => e.Error).Returns((Exception)null!);
            _httpContext.Features.Set(exceptionFeatureMock.Object);

            // Act
            Action act = () => _controller.Error();

            // Assert
            act.Should().Throw<Exception>();
        }
    }
}