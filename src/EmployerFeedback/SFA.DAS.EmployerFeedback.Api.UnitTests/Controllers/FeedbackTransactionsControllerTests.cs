using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Api.Controllers;
using SFA.DAS.EmployerFeedback.Application.Commands.TriggerFeedbackEmails;
using SFA.DAS.EmployerFeedback.Application.Queries.GetFeedbackTransactionsBatch;
using SFA.DAS.EmployerFeedback.Models;
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

        [Test]
        public async Task TriggerFeedbackEmails_ReturnsNoContent_WhenRequestIsValid()
        {
            var feedbackTransactionId = 123L;
            var request = new TriggerFeedbackEmailsRequest
            {
                NotificationTemplates = new List<NotificationTemplateRequest>
                {
                    new NotificationTemplateRequest
                    {
                        TemplateName = "TestTemplate",
                        TemplateId = Guid.NewGuid()
                    }
                },
                EmployerAccountsBaseUrl = "https://test.com"
            };

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<TriggerFeedbackEmailsCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.TriggerFeedbackEmails(feedbackTransactionId, request);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
            _mediatorMock.Verify(x => x.Send(It.Is<TriggerFeedbackEmailsCommand>(cmd =>
                cmd.FeedbackTransactionId == feedbackTransactionId &&
                cmd.Request == request
            ), CancellationToken.None), Times.Once);
        }

        [Test]
        public async Task TriggerFeedbackEmails_ReturnsBadRequest_WhenSendAfterDateIsInFuture()
        {
            var feedbackTransactionId = 124L;
            var request = new TriggerFeedbackEmailsRequest
            {
                NotificationTemplates = new List<NotificationTemplateRequest>
                {
                    new NotificationTemplateRequest { TemplateName = "TestTemplate", TemplateId = Guid.NewGuid() }
                }
            };

            var exception = new InvalidOperationException("Feedback transaction sendAfter date is in the future");
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<TriggerFeedbackEmailsCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            var result = await _controller.TriggerFeedbackEmails(feedbackTransactionId, request);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            var errorResponse = badRequestResult.Value;
            Assert.That(errorResponse.GetType().GetProperty("error").GetValue(errorResponse),
                Is.EqualTo("Feedback transaction sendAfter date is in the future"));
        }

        [Test]
        public async Task TriggerFeedbackEmails_ReturnsBadRequest_WhenNoMatchingTemplateFound()
        {
            var feedbackTransactionId = 125L;
            var request = new TriggerFeedbackEmailsRequest
            {
                NotificationTemplates = new List<NotificationTemplateRequest>
                {
                    new NotificationTemplateRequest { TemplateName = "TestTemplate", TemplateId = Guid.NewGuid() }
                }
            };

            var exception = new InvalidOperationException("No matching template found for templateName: TestTemplate");
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<TriggerFeedbackEmailsCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            var result = await _controller.TriggerFeedbackEmails(feedbackTransactionId, request);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            var errorResponse = badRequestResult.Value;
            Assert.That(errorResponse.GetType().GetProperty("error").GetValue(errorResponse),
                Is.EqualTo("No matching template found for templateName: TestTemplate"));
        }

        [Test]
        public async Task TriggerFeedbackEmails_ReturnsInternalServerError_WhenUnexpectedExceptionOccurs()
        {
            var feedbackTransactionId = 126L;
            var request = new TriggerFeedbackEmailsRequest
            {
                NotificationTemplates = new List<NotificationTemplateRequest>
                {
                    new NotificationTemplateRequest { TemplateName = "TestTemplate", TemplateId = Guid.NewGuid() }
                }
            };

            var exception = new Exception("Unexpected error occurred");
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<TriggerFeedbackEmailsCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            var result = await _controller.TriggerFeedbackEmails(feedbackTransactionId, request);

            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusResult = result as StatusCodeResult;
            Assert.That(statusResult.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
    }
}