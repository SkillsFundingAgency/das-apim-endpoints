using AutoFixture;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Api.Controllers;
using SFA.DAS.Aodp.Application.Queries.Application.Form;

namespace SFA.DAS.Aodp.Api.UnitTests.Controllers
{
    [TestFixture]
    public class BaseControllerTests
    {
        private IFixture _fixture;
        private Mock<ILogger<BaseController>> _loggerMock;
        private Mock<IMediator> _mediatorMock;
        private BaseController _controller;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _loggerMock = _fixture.Freeze<Mock<ILogger<BaseController>>>();
            _mediatorMock = _fixture.Freeze<Mock<IMediator>>();
            _controller = new BaseController(_mediatorMock.Object, _loggerMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [Test]
        public async Task Get_Response_Success()
        {
            // Arrange
            var request = _fixture.Create<GetApplicationFormByIdQuery>();
            var response = _fixture.Create<GetApplicationFormByIdQueryResponse>();
            BaseMediatrResponse<GetApplicationFormByIdQueryResponse> wrapper = new()
            {
                Value = response,
                Success = true
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetApplicationFormByIdQuery>(), default))
                .ReturnsAsync(wrapper);

            // Act
            var result = await _controller.SendRequestAsync(request);

            // Assert
            _mediatorMock.Verify(m => m.Send(request, default), Times.Once());

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.AssignableFrom<GetApplicationFormByIdQueryResponse>());
            var model = (GetApplicationFormByIdQueryResponse)okResult.Value;

            Assert.That(response.FormTitle, Is.EqualTo(model.FormTitle));
        }

        [Test]
        public async Task Get_Response_ErrorThrownHandlingRequest()
        {
            // Arrange
            var request = _fixture.Create<GetApplicationFormByIdQuery>();
            var response = _fixture.Create<GetApplicationFormByIdQueryResponse>();
            BaseMediatrResponse<GetApplicationFormByIdQueryResponse> wrapper = new()
            {
                Value = response,
                Success = false,
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetApplicationFormByIdQuery>(), default))
                .ReturnsAsync(wrapper);

            // Act
            var result = await _controller.SendRequestAsync(request);

            // Assert
            _mediatorMock.Verify(m => m.Send(request, default), Times.Once());
            Assert.That(result, Is.InstanceOf<StatusCodeResult>());

            var statusCodeResult = (StatusCodeResult)result;
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(500));

        }

    }
}