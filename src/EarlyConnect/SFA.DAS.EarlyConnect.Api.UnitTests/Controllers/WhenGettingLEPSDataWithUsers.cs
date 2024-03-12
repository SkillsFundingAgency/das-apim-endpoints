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
using SFA.DAS.EarlyConnect.Application.Queries.GetLEPSDataWithUsers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EarlyConnect.Api.UnitTests.Controllers
{
    [TestFixture]
    public class WhenGettingLEPSDataWithUsers
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<LepsDataController>> _loggerMock;

        private LepsDataController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<LepsDataController>>();
            _controller = new LepsDataController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test, MoqAutoData]
        public async Task GetLepsData_ValidLepsCode_ReturnsOk(
            GetLEPSDataWithUsersResult mediatorResult
        )
        {
            _mediatorMock
                          .Setup(m => m.Send(It.IsAny<GetLEPSDataWithUsersQuery>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(mediatorResult);

            var result = await _controller.GetLepsDataWithUsers();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<GetLEPSDataListWithUsersResponse>());
            Assert.That((int)HttpStatusCode.OK, Is.EqualTo(okResult.StatusCode));
        }

        [Test]
        public async Task GetMetricsData_InvalidLepsCode_ReturnsBadRequest()
        {
            var exception = new Exception("Simulated error");
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetLEPSDataWithUsersQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(exception);

            var result = await _controller.GetLepsDataWithUsers();

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.That((int)HttpStatusCode.BadRequest, Is.EqualTo(badRequestResult.StatusCode));
        }
    }
}
