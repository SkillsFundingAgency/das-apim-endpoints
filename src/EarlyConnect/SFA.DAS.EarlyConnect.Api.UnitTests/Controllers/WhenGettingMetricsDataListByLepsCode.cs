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
using SFA.DAS.EarlyConnect.Application.Queries.GetMetricsDataByLepsCode;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EarlyConnect.Api.UnitTests.Controllers
{
    [TestFixture]
    public class WhenGettingMetricsDataListByLepsCode
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<MetricDataController>> _loggerMock;

        private MetricDataController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<MetricDataController>>();
            _controller = new MetricDataController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test, MoqAutoData]
        public async Task GetMetricsData_ValidLepsCode_ReturnsOk(
            GetMetricsDataByLepsCodeResult mediatorResult
        )
        {
            var lepsCode = "ValidLepsCode";
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetMetricsDataByLepsCodeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var result = await _controller.GetMetricsData(lepsCode);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.IsInstanceOf<GetMetricsDataListByLepsCodeResponse>(okResult.Value);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Test]
        public async Task GetMetricsData_InvalidLepsCode_ReturnsBadRequest()
        {
            var lepsCode = "InvalidLepsCode";
            var exception = new Exception("Simulated error");
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetMetricsDataByLepsCodeQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);

            var result = await _controller.GetMetricsData(lepsCode);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        }
    }
}
