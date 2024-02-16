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
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EarlyConnect.Api.UnitTests.Controllers
{
    [TestFixture]
    public class WhenGettingStudentTriageDataBySurveyId
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<StudentTriageDataController>> _loggerMock;

        private StudentTriageDataController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<StudentTriageDataController>>();
            _controller = new StudentTriageDataController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test, MoqAutoData]
        public async Task GetStudentTriageData_ValidRequest_ReturnsOk(
            GetStudentTriageDataBySurveyIdResult mediatorResult,
            Guid surveyGuid
        )
        {
            _mediatorMock
                          .Setup(m => m.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(mediatorResult);

            var result = await _controller.GetStudentTriageData(surveyGuid);

            Assert.That(result, Is.lnstanceOf<OkOBjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.IsInstanceOf<GetStudentTriageDataBySurveyIdResponse>(okResult.Value);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Test]
        public async Task GetStudentTriageData_InvalidRequest_ReturnsBadRequest()
        {
            Guid surveyId = Guid.NewGuid();
            var exception = new Exception("Simulated error");
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetStudentTriageDataBySurveyIdQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);

            var result = await _controller.GetStudentTriageData(surveyId);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        }
    }
}
