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
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;

namespace SFA.DAS.EarlyConnect.Api.UnitTests.Controllers

{
    [TestFixture]
    public class WhenPostingStudentTriageData
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
            var request = new CreateOtherStudentTriageDataPostRequest();

            var createOtherStudentTriageDataCommandResult = new CreateOtherStudentTriageDataCommandResult { StudentSurveyId = "Test", AuthCode = "Test", ExpiryDate = DateTime.Now };

            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateOtherStudentTriageDataCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(createOtherStudentTriageDataCommandResult));

            var result = await _controller.CreateStudentTriageData(request);

            Assert.That(result, Is.InstanceOf<CreatedAtActionResult>());
            var okResult = (CreatedAtActionResult)result;

            Assert.That(okResult.StatusCode, Is.EqualTo((int)HttpStatusCode.Created));
            var model = (CreateOtherStudentTriageDataPostResponse)okResult.Value;
            Assert.That(model.StudentSurveyId, Is.EqualTo(createOtherStudentTriageDataCommandResult.StudentSurveyId));
            Assert.That(model.AuthCode, Is.EqualTo(createOtherStudentTriageDataCommandResult.AuthCode));
            Assert.That(model.Expiry, Is.EqualTo(createOtherStudentTriageDataCommandResult.ExpiryDate));
        }

        [Test]
        public async Task Post_ExceptionThrown_ReturnsBadRequestResult()
        {
            var request = new CreateOtherStudentTriageDataPostRequest();

            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateOtherStudentTriageDataCommand>(), It.IsAny<CancellationToken>())).Throws(new Exception());

            var result = await _controller.CreateStudentTriageData(request);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}

