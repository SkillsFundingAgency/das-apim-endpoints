using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Api.Controllers;
using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.Application.Commands.DeliveryUpdateData;


namespace SFA.DAS.EarlyConnect.Api.UnitTests.Controllers

{
    [TestFixture]
    public class WhenPostingDeliveryUpdateData
    {
        private DeliveryUpdateController _controller;
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<DeliveryUpdateController>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<DeliveryUpdateController>>();
            _controller = new DeliveryUpdateController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Post_DeliveryUpdate_ValidRequest_ReturnsOkResult()
        {
            var request = new DeliveryUpdatePostRequest();
            var response = new DeliveryUpdateCommandResult { Message = "Success" };

            _mediatorMock.Setup(x => x.Send(It.IsAny<DeliveryUpdateCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _controller.DeliveryUpdate(request);

            Assert.That(result, Is.InstanceOf<OkResult>());
            var okResult = (OkResult)result;

            Assert.That(okResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        }

        [Test]
        public async Task Post_DeliveryUpdate_ExceptionThrown_ReturnsBadRequestResult()
        {
            var request = new DeliveryUpdatePostRequest();

            _mediatorMock.Setup(x => x.Send(It.IsAny<DeliveryUpdateCommand>(), It.IsAny<CancellationToken>())).Throws(new Exception());

            var result = await _controller.DeliveryUpdate(request);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = (BadRequestObjectResult)result;

            Assert.That(badRequestResult.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));

        }
        
    }
}

