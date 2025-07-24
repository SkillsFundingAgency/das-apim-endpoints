using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Api.Controllers;
using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataByDate;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EarlyConnect.Api.UnitTests.Controllers
{
    [TestFixture]
    public class WhenGettingStudentTriageDataByDate
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
            List<GetStudentTriageDataResponse> mediatorResult,
            DateTime toDate,
            DateTime fromDate
        )
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetStudentTriageDataByDateQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var result = await _controller.ResendDataToLondon(toDate, fromDate);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<List<GetStudentTriageDataResponse>>());
            Assert.That(okResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task GetStudentTriageData_InvalidRequest_ReturnsBadRequest()
        {
            var toDate = DateTime.UtcNow;
            var fromDate = DateTime.UtcNow;
            var exception = new Exception("Simulated error");
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetStudentTriageDataByDateQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);

            var result = await _controller.ResendDataToLondon(toDate, fromDate);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.That(badRequestResult.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }
    }
}
