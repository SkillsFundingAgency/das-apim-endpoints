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
using SFA.DAS.EmployerFeedback.Application.Commands.SubmitEmployerFeedback;
using SFA.DAS.EmployerFeedback.Models;

namespace SFA.DAS.EmployerFeedback.Api.UnitTests.Controllers
{
    [TestFixture]
    public class EmployerFeedbackResultControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<EmployerFeedbackResultController>> _loggerMock;
        private EmployerFeedbackResultController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<EmployerFeedbackResultController>>();
            _controller = new EmployerFeedbackResultController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task SubmitEmployerFeedback_ReturnsNoContent_WhenCommandSucceeds()
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SubmitEmployerFeedbackCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var request = new SubmitEmployerFeedbackRequest
            {
                UserRef = Guid.NewGuid(),
                Ukprn = 123,
                AccountId = 456,
                ProviderRating = OverallRating.Good,
                FeedbackSource = 1,
                ProviderAttributes = null
            };

            var result = await _controller.SubmitEmployerFeedback(request);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
            _mediatorMock.Verify(m => m.Send(
                It.Is<SubmitEmployerFeedbackCommand>(c =>
                    c.UserRef == request.UserRef &&
                    c.Ukprn == request.Ukprn &&
                    c.AccountId == request.AccountId &&
                    c.ProviderRating == request.ProviderRating &&
                    c.FeedbackSource == request.FeedbackSource &&
                    c.ProviderAttributes == request.ProviderAttributes),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task SubmitEmployerFeedback_ReturnsInternalServerError_AndLogs_WhenExceptionThrown()
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SubmitEmployerFeedbackCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("fail"));

            var request = new SubmitEmployerFeedbackRequest
            {
                UserRef = Guid.NewGuid(),
                Ukprn = 123,
                AccountId = 456,
                ProviderRating = OverallRating.Good,
                FeedbackSource = 1,
                ProviderAttributes = null
            };

            var result = await _controller.SubmitEmployerFeedback(request);

            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusResult = result as StatusCodeResult;
            Assert.That(statusResult.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Unhandled error submitting employer feedback.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
