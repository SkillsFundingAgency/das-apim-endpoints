using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Api.Controllers;
using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.Application.Commands.CreateMetricData;

namespace SFA.DAS.EarlyConnect.Api.UnitTests.Controllers

{
    [TestFixture]
    public class WhenPostingMetricData
    {
        private MetricDataController _controller;
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<MetricDataController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<MetricDataController>>();
            _controller = new MetricDataController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Post_ValidRequest_ReturnsOkResult()
        {
            var request = new CreateMetricDataPostRequest
            {
                MetricsData = new List<MetricRequestModel>()
            };

            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateMetricDataCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(Unit.Value));

            var result = await _controller.CreateMetricsData(request);

            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task Post_ExceptionThrown_ReturnsBadRequestResult()
        {
            var request = new CreateMetricDataPostRequest
            {
                MetricsData = new List<MetricRequestModel>()
            };

            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateMetricDataCommand>(), It.IsAny<CancellationToken>())).Throws(new Exception());

            var result = await _controller.CreateMetricsData(request);

            Assert.IsInstanceOf<BadRequestResult>(result);
        }
    }
}

