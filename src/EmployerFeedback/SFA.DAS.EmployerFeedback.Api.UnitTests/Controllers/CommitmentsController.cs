using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Api.Controllers;
using SFA.DAS.EmployerFeedback.Application.Queries.GetProvider;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;


//TODO - REMOVE GETPROVIDER TESTS - TEMPORARY - FOR TESTING CONNECTIVITY PURPOSES ONLY
namespace SFA.DAS.EmployerFeedback.Api.UnitTests.Controllers
{
    [TestFixture]
    public class CommitmentsControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<CommitmentsController>> _loggerMock;
        private CommitmentsController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<CommitmentsController>>();
            _controller = new CommitmentsController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetProvider_ReturnsOk_WhenIdIsNotNull()
        {
            var providerId = 12345678;
            var resultObj = new GetProviderQueryResult { Name = "", ProviderId = 12345678 };
            _mediatorMock.Setup(m => m.Send(It.Is<GetProviderQuery>(q => q != null), CancellationToken.None)).ReturnsAsync(resultObj);

            var result = await _controller.GetProvider(providerId);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(resultObj));
        }

        [Test]
        public async Task GetProvider_ReturnsNotFound_WhenIdIsZero()
        {
            var resultObj = new GetProviderQueryResult();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProviderQuery>(), CancellationToken.None)).ReturnsAsync(resultObj);

            var result = await _controller.GetProvider(0);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetProvider_ReturnsNotFound_WhenIdIsNegative()
        {
            var resultObj = new GetProviderQueryResult();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProviderQuery>(), CancellationToken.None)).ReturnsAsync(resultObj);

            var result = await _controller.GetProvider(-1);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetProvider_ReturnsInternalServerError_WhenExceptionThrown()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProviderQuery>(), CancellationToken.None)).ThrowsAsync(new Exception());

            var result = await _controller.GetProvider(12345678);

            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusCodeResult = result as StatusCodeResult;
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }

        [Test]
        public async Task GetProvider_ProviderResponseIsNull_ReturnsNotFound()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProviderQuery>(), CancellationToken.None)).ReturnsAsync((GetProviderQueryResult)null);
            var result = await _controller.GetProvider(12345678);
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}