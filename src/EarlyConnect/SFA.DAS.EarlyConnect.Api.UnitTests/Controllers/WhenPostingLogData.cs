using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Api.Controllers;
using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.Application.Commands.CreateLogData;
using SFA.DAS.EarlyConnect.Application.Commands.UpdateLogData;

namespace SFA.DAS.EarlyConnect.Api.UnitTests.Controllers

{
    [TestFixture]
    public class WhenPostingLogData
    {
        private LogDataController _controller;
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<LogDataController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<LogDataController>>();
            _controller = new LogDataController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Post_CreateLog_ValidRequest_ReturnsOkResult()
        {
            var request = new CreateLogPostRequest();
            var response = new CreateLogDataCommandResult { LogId = 1 };

            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateLogDataCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _controller.CreateLog(request);

            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var okResult = (CreatedAtActionResult)result;

            Assert.AreEqual((int)HttpStatusCode.Created, okResult.StatusCode);
            var model = (CreateLogPostResponse)okResult.Value;
            Assert.AreEqual(response.LogId, model.LogId);
        }

        [Test]
        public async Task Post_CreateLog_ExceptionThrown_ReturnsBadRequestResult()
        {
            var request = new CreateLogPostRequest();

            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateLogDataCommand>(), It.IsAny<CancellationToken>())).Throws(new Exception());

            var result = await _controller.CreateLog(request);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public async Task Post_UpdateLog_ValidRequest_ReturnsOkResult()
        {
            var request = new UpdateLogPostRequest();

            _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateLogDataCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(Unit.Value));

            var result = await _controller.UpdateLog(request);

            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task Post_UpdateLog_ExceptionThrown_ReturnsBadRequestResult()
        {
            var request = new UpdateLogPostRequest();

            _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateLogDataCommand>(), It.IsAny<CancellationToken>())).Throws(new Exception());

            var result = await _controller.UpdateLog(request);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }
    }
}

