using System;
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
using SFA.DAS.EarlyConnect.Application.Commands.SendReminderEmail;

namespace SFA.DAS.EarlyConnect.Api.UnitTests.Controllers

{
    [TestFixture]
    public class WhenPostingReminderEmail
    {
        private StudentTriageDataController _controller;
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<StudentTriageDataController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<StudentTriageDataController>>();

            _controller = new StudentTriageDataController(_mediatorMock.Object, _loggerMock.Object)
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
            var request = new SendReminderEmailRequest();
            request.LepsCode = "ABC123";
            var sendReminderEmailCommandResult = new SendReminderEmailCommandResult { Message="Success"};

            _mediatorMock.Setup(x => x.Send(It.IsAny<SendReminderEmailCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(sendReminderEmailCommandResult));

            var result = await _controller.StudentSurveyEmailReminder(request);

            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var okResult = (CreatedAtActionResult)result;                    

            var mockResult = new SendReminderEmailResponse
            {
                Message = "Success"
            };

            Assert.That(okResult.StatusCode, Is.EqualTo((int)HttpStatusCode.Created));
            var model = (SendReminderEmailResponse)mockResult;
            Assert.That(model.Message, Is.EqualTo(sendReminderEmailCommandResult.Message));
        }

        [Test]
        public async Task Post_ExceptionThrown_ReturnsBadRequestResult()
        {
            var request = new SendReminderEmailRequest();

            _mediatorMock.Setup(x => x.Send(It.IsAny<SendReminderEmailCommand>(), It.IsAny<CancellationToken>())).Throws(new Exception());

            var result = await _controller.StudentSurveyEmailReminder(request);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}

