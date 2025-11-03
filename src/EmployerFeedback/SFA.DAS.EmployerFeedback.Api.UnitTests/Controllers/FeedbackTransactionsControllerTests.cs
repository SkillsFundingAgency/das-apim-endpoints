using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Api.Controllers;
using SFA.DAS.EmployerFeedback.Application.Queries.GetFeedbackTransactionsBatch;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Api.UnitTests.Controllers
{
    [TestFixture]
    public class FeedbackTransactionsControllerTests
    {
        private Mock<ILogger<FeedbackTransactionsController>> _loggerMock;
        private Mock<IMediator> _mediatorMock;
        private FeedbackTransactionsController _controller;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<FeedbackTransactionsController>>();
            _mediatorMock = new Mock<IMediator>();
            _controller = new FeedbackTransactionsController(_mediatorMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetFeedbackTransactionsBatch_ReturnsOk_WhenFeedbackTransactionsAreNotNull()
        {
            var batchSize = 10;
            var feedbackTransactions = new List<long> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var resultObj = new GetFeedbackTransactionsBatchResult { FeedbackTransactions = feedbackTransactions };
            _mediatorMock.Setup(m => m.Send(It.Is<GetFeedbackTransactionsBatchQuery>(q => q.BatchSize == batchSize), CancellationToken.None)).ReturnsAsync(resultObj);

            var result = await _controller.GetFeedbackTransactionsBatch(batchSize);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var response = okResult.Value;
            Assert.That(response, Is.Not.Null);
            var feedbackTransactionsProperty = response.GetType().GetProperty("feedbackTransactions");
            Assert.That(feedbackTransactionsProperty, Is.Not.Null);
            Assert.That(feedbackTransactionsProperty.GetValue(response), Is.EqualTo(feedbackTransactions));
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetFeedbackTransactionsBatchQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetFeedbackTransactionsBatch_ReturnsNotFound_WhenFeedbackTransactionsAreNull()
        {
            var batchSize = 10;
            var resultObj = new GetFeedbackTransactionsBatchResult { FeedbackTransactions = null };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetFeedbackTransactionsBatchQuery>(), CancellationToken.None)).ReturnsAsync(resultObj);

            var result = await _controller.GetFeedbackTransactionsBatch(batchSize);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetFeedbackTransactionsBatchQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetFeedbackTransactionsBatch_ReturnsInternalServerError_AndLogs_WhenExceptionThrown()
        {
            var batchSize = 10;
            var boom = new InvalidOperationException("test");
            _mediatorMock
                .Setup(m => m.Send(It.Is<GetFeedbackTransactionsBatchQuery>(q => q.BatchSize == batchSize), CancellationToken.None))
                .ThrowsAsync(boom);

            var result = await _controller.GetFeedbackTransactionsBatch(batchSize);

            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusResult = result as StatusCodeResult;
            Assert.That(statusResult.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error getting feedback transactions batch.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}