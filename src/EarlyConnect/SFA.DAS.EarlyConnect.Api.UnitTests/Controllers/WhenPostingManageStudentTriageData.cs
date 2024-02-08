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
using SFA.DAS.EarlyConnect.Application.Commands.ManageStudentTriageData;

namespace SFA.DAS.EarlyConnect.Api.UnitTests.Controllers

{
    [TestFixture]
    public class WhenPostingManageStudentTriageData
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
            var request = new ManageStudentTriageDataPostRequest();
            request.StudentSurvey = new StudentSurveyRequest();
            request.StudentSurvey.ResponseAnswers = new List<AnswersRequest>();


            var manageStudentTriageDataCommandResult = new ManageStudentTriageDataCommandResult { Message = "Success"};

            _mediatorMock.Setup(x => x.Send(It.IsAny<ManageStudentTriageDataCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(manageStudentTriageDataCommandResult));

            var result = await _controller.ManageStudentTriageData(request, new Guid());

            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var okResult = (CreatedAtActionResult)result;

            Assert.AreEqual((int)HttpStatusCode.Created, okResult.StatusCode);
            var model = (ManageStudentTriageDataPostResponse)okResult.Value;
            Assert.AreEqual(manageStudentTriageDataCommandResult.Message, model.Message);
        }

        [Test]
        public async Task Post_ExceptionThrown_ReturnsBadRequestResult()
        {
            var request = new ManageStudentTriageDataPostRequest();
            request.StudentSurvey = new StudentSurveyRequest();

            _mediatorMock.Setup(x => x.Send(It.IsAny<ManageStudentTriageDataCommand>(), It.IsAny<CancellationToken>())).Throws(new Exception());

            var result = await _controller.ManageStudentTriageData(request, new Guid());

            Assert.IsInstanceOf<BadRequestResult>(result);
        }
    }
}

