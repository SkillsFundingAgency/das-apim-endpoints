using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Api.Controllers;
using SFA.DAS.EmployerFeedback.Application.Queries.GetFeedbackTransactionUsers;
using SFA.DAS.EmployerFeedback.Application.Queries.GetFeedbackTransactionsBatch;
using SFA.DAS.EmployerFeedback.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerFeedback.Application.Commands.SendFeedbackEmail;
using SFA.DAS.EmployerFeedback.Application.Commands.UpdateFeedbackTransaction;

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
        public async Task SendFeedbackEmail_ReturnsNoContent_WhenRequestIsValid()
        {
            var request = new SendFeedbackEmailRequest
            {
                TemplateId = Guid.NewGuid(),
                Contact = "Test Contact",
                EmployerName = "Test Employer",
                AccountHashedId = "ABCD123",
                AccountsBaseUrl = "https://test.com"
            };

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<SendFeedbackEmailCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.SendFeedbackEmail(request);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
            _mediatorMock.Verify(x => x.Send(It.Is<SendFeedbackEmailCommand>(cmd =>
                cmd.Request == request
            ), CancellationToken.None), Times.Once);
        }

        [Test]
        public async Task SendFeedbackEmail_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            var request = new SendFeedbackEmailRequest
            {
                TemplateId = Guid.NewGuid(),
                Contact = "Test Contact",
                EmployerName = "Test Employer",
                AccountHashedId = "ABCD123",
                AccountsBaseUrl = "https://test.com"
            };

            var exception = new Exception("Unexpected error occurred");
            _mediatorMock
                .Setup(x => x.Send(It.IsAny<SendFeedbackEmailCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            var result = await _controller.SendFeedbackEmail(request);

            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusResult = result as StatusCodeResult;
            Assert.That(statusResult.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }

        [Test]
        public async Task GetFeedbackTransactionUsers_ReturnsOk_WhenUsersFound()
        {
            var feedbackTransactionId = 123L;
            var queryResult = new GetFeedbackTransactionUsersResult
            {
                AccountId = 456,
                AccountName = "Test Account",
                TemplateName = "TestTemplate",
                Users = new List<FeedbackTransactionUser>
                {
                    new FeedbackTransactionUser { Name = "Test User", Email = "test@example.com" }
                }
            };

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetFeedbackTransactionUsersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var result = await _controller.GetFeedbackTransactionUsers(feedbackTransactionId);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var response = okResult.Value as GetFeedbackTransactionUsersResult;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.AccountId, Is.EqualTo(456));
            Assert.That(response.AccountName, Is.EqualTo("Test Account"));
            Assert.That(response.TemplateName, Is.EqualTo("TestTemplate"));
            Assert.That(response.Users.Count(), Is.EqualTo(1));
            _mediatorMock.Verify(x => x.Send(It.Is<GetFeedbackTransactionUsersQuery>(q =>
                q.FeedbackTransactionId == feedbackTransactionId
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetFeedbackTransactionUsers_ReturnsNoContent_WhenAlreadySent()
        {
            var feedbackTransactionId = 124L;

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetFeedbackTransactionUsersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GetFeedbackTransactionUsersResult)null);

            var result = await _controller.GetFeedbackTransactionUsers(feedbackTransactionId);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
            _mediatorMock.Verify(x => x.Send(It.Is<GetFeedbackTransactionUsersQuery>(q =>
                q.FeedbackTransactionId == feedbackTransactionId
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetFeedbackTransactionUsers_ReturnsBadRequest_WhenSendAfterDateIsInFuture()
        {
            var feedbackTransactionId = 125L;
            var exception = new InvalidOperationException("Feedback transaction sendAfter date is in the future");

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetFeedbackTransactionUsersQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            var result = await _controller.GetFeedbackTransactionUsers(feedbackTransactionId);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            var errorResponse = badRequestResult.Value;
            Assert.That(errorResponse.GetType().GetProperty("error").GetValue(errorResponse),
                Is.EqualTo("Feedback transaction sendAfter date is in the future"));
            _mediatorMock.Verify(x => x.Send(It.Is<GetFeedbackTransactionUsersQuery>(q =>
                q.FeedbackTransactionId == feedbackTransactionId
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetFeedbackTransactionUsers_ReturnsInternalServerError_WhenUnexpectedExceptionOccurs()
        {
            var feedbackTransactionId = 126L;
            var exception = new Exception("Unexpected error occurred");

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetFeedbackTransactionUsersQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            var result = await _controller.GetFeedbackTransactionUsers(feedbackTransactionId);

            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusResult = result as StatusCodeResult;
            Assert.That(statusResult.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
            _mediatorMock.Verify(x => x.Send(It.Is<GetFeedbackTransactionUsersQuery>(q =>
                q.FeedbackTransactionId == feedbackTransactionId
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateFeedbackTransaction_ReturnsNoContent_WhenRequestIsValid()
        {
            var feedbackTransactionId = 123L;
            var request = new UpdateFeedbackTransactionRequest
            {
                TemplateId = Guid.NewGuid(),
                SentCount = 5,
                SentDate = DateTime.UtcNow
            };

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<UpdateFeedbackTransactionCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.UpdateFeedbackTransaction(feedbackTransactionId, request);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
            _mediatorMock.Verify(x => x.Send(It.Is<UpdateFeedbackTransactionCommand>(cmd =>
                cmd.Id == feedbackTransactionId &&
                cmd.Request == request
            ), CancellationToken.None), Times.Once);
        }

        [Test]
        public async Task UpdateFeedbackTransaction_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            var feedbackTransactionId = 123L;
            var request = new UpdateFeedbackTransactionRequest
            {
                TemplateId = Guid.NewGuid(),
                SentCount = 5,
                SentDate = DateTime.UtcNow
            };
            var exception = new Exception("Unexpected error occurred");

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<UpdateFeedbackTransactionCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            var result = await _controller.UpdateFeedbackTransaction(feedbackTransactionId, request);

            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusResult = result as StatusCodeResult;
            Assert.That(statusResult.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }
    }
}