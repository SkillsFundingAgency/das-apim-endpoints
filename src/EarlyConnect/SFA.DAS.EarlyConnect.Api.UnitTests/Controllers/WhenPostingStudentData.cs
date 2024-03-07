using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Api.Controllers;
using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.Application.Commands.CreateLogData;
using SFA.DAS.EarlyConnect.Application.Commands.CreateStudentData;
using SFA.DAS.EarlyConnect.Application.Commands.UpdateLogData;

namespace SFA.DAS.EarlyConnect.Api.UnitTests.Controllers

{
    [TestFixture]
    public class WhenPostingStudentData
    {
        private StudentDataController _controller;
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<StudentDataController>> _loggerMock;
        private Mock<HttpContext> _httpContextMock;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<StudentDataController>>();
            _httpContextMock = new Mock<HttpContext>();

            _controller = new StudentDataController(_mediatorMock.Object, _loggerMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor(),
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Test]
        public async Task Post_ValidRequest_ReturnsOkResult()
        {
            var request = new CreateStudentDataPostRequest
            {
                ListOfStudentData = new List<StudentRequestModel>()
            };

            var createLogDataCommandResult = new CreateLogDataCommandResult { LogId = 1 };
            var createStudentDataCommandResult = new CreateStudentDataCommandResult { Message = "Test" };

            _httpContextMock.SetupGet(x => x.Connection.RemoteIpAddress).Returns(new IPAddress(new byte[] { 127, 0, 0, 1 }));
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateLogDataCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(createLogDataCommandResult);
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentDataCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(createStudentDataCommandResult));
            _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateLogDataCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(Unit.Value));

            var result = await _controller.CreateStudentData(request);

            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
        }

        [Test]
        public async Task Post_ExceptionThrown_ReturnsBadRequestResult()
        {
            var request = new CreateStudentDataPostRequest
            {
                ListOfStudentData = new List<StudentRequestModel>()
            };

            var createLogDataCommandResult = new CreateLogDataCommandResult { LogId = 1 };

            _httpContextMock.SetupGet(x => x.Connection.RemoteIpAddress).Returns(new IPAddress(new byte[] { 127, 0, 0, 1 }));
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateLogDataCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(createLogDataCommandResult);
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateStudentDataCommand>(), It.IsAny<CancellationToken>())).Throws(new Exception());

            var result = await _controller.CreateStudentData(request);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}

