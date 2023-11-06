using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Api.Controllers;
using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.Application.Commands.CreateLog;
using SFA.DAS.EarlyConnect.Application.Commands.UpdateLog;

namespace SFA.DAS.EarlyConnect.Api.UnitTests.Controllers

{
    [TestFixture]
    public class WhenPostingLog
    {
        private LogController _controller;
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<LogController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<LogController>>();
            _controller = new LogController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Post_CreateLog_ValidRequest_ReturnsOkResult()
        {
            var request = new CreateLogPostRequest();

            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateLogCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(Unit.Value));

            var result = await _controller.CreateLog(request);

            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task Post_CreateLog_ExceptionThrown_ReturnsBadRequestResult()
        {
            var request = new CreateLogPostRequest();

            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateLogCommand>(), It.IsAny<CancellationToken>())).Throws(new Exception());

            var result = await _controller.CreateLog(request);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public async Task Post_UpdateLog_ValidRequest_ReturnsOkResult()
        {
            var request = new UpdateLogPostRequest();

            _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateLogCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(Unit.Value));

            var result = await _controller.UpdateLog(request);

            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task Post_UpdateLog_ExceptionThrown_ReturnsBadRequestResult()
        {
            var request = new UpdateLogPostRequest();

            _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateLogCommand>(), It.IsAny<CancellationToken>())).Throws(new Exception());

            var result = await _controller.UpdateLog(request);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }
    }
}

