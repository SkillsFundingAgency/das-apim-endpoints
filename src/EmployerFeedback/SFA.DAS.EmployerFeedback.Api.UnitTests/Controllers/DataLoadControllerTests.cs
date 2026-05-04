using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Api.Controllers;
using SFA.DAS.EmployerFeedback.Api.UnitTests.Extensions;
using SFA.DAS.EmployerFeedback.Application.Commands.GenerateFeedbackSummaries;

namespace SFA.DAS.EmployerFeedback.Api.UnitTests.Controllers
{
    [TestFixture]
    public class DataLoadControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<DataLoadController>> _loggerMock;
        private DataLoadController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<DataLoadController>>();
            _controller = new DataLoadController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GenerateFeedbackSummaries_ReturnsNoContent_WhenCommandSucceeds()
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GenerateFeedbackSummariesCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.GenerateFeedbackSummaries();

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task GenerateFeedbackSummaries_ReturnsInternalServerError_AndLogs_WhenExceptionThrown()
        {
            var boom = new InvalidOperationException("boom");
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GenerateFeedbackSummariesCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(boom);

            var result = await _controller.GenerateFeedbackSummaries();

            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusResult = result as StatusCodeResult;
            Assert.That(statusResult.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

            _loggerMock.VerifyLogErrorContains("Error generating feedback summaries.", boom, Times.Once());
        }
    }
}
