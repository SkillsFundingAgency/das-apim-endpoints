using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Api.Controllers;
using SFA.DAS.EmployerFeedback.Api.UnitTests.Extensions;
using SFA.DAS.EmployerFeedback.Application.Queries.GetAttributes;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Api.UnitTests.Controllers
{
    [TestFixture]
    public class AttributesControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<AttributesController>> _loggerMock;
        private AttributesController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<AttributesController>>();
            _controller = new AttributesController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOk_WhenAttributesAreNotNull()
        {
            var resultObj = new GetAttributesResult { Attributes = new() { new() { AttributeId = 1, AttributeName = "Test" } } };
            _mediatorMock.Setup(m => m.Send(It.Is<GetAttributesQuery>(q => q != null), CancellationToken.None)).ReturnsAsync(resultObj);

            var result = await _controller.GetAll();

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(resultObj.Attributes));
        }

        [Test]
        public async Task GetAll_ReturnsNotFound_WhenAttributesAreNull()
        {
            var resultObj = new GetAttributesResult { Attributes = null };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAttributesQuery>(), CancellationToken.None)).ReturnsAsync(resultObj);

            var result = await _controller.GetAll();

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetAll_ReturnsInternalServerError_AndLogs_WhenExceptionThrown()
        {
            var boom = new InvalidOperationException("boom");
            _mediatorMock
                .Setup(m => m.Send(It.Is<GetAttributesQuery>(q => q != null), CancellationToken.None))
                .ThrowsAsync(boom);

            var result = await _controller.GetAll();

            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusResult = result as StatusCodeResult;
            Assert.That(statusResult.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

            _loggerMock.VerifyLogErrorContains("Error attempting to retrieve attributes.", boom, Times.Once());
        }
    }
}
