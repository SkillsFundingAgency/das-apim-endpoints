using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Api.Controllers;
using SFA.DAS.EmployerFeedback.Api.TaskQueue;
using SFA.DAS.EmployerFeedback.Api.UnitTests.Extensions;
using SFA.DAS.EmployerFeedback.Application.Commands.SyncEmployerAccounts;
using SFA.DAS.EmployerFeedback.Application.Commands.UpsertFeedbackTransaction;
using SFA.DAS.EmployerFeedback.Application.Queries.GetAccountsBatch;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Api.UnitTests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Mock<IBackgroundTaskQueue> _backgroundTaskQueueMock;
        private Mock<ILogger<AccountController>> _loggerMock;
        private Mock<IMediator> _mediatorMock;
        private AccountController _controller;

        [SetUp]
        public void SetUp()
        {
            _backgroundTaskQueueMock = new Mock<IBackgroundTaskQueue>();
            _loggerMock = new Mock<ILogger<AccountController>>();
            _mediatorMock = new Mock<IMediator>();
            _controller = new AccountController(_loggerMock.Object, _backgroundTaskQueueMock.Object, _mediatorMock.Object);
        }

        [Test]
        public void SyncEmployerAccounts_ReturnsNoContent_WhenQueuedSuccessfully()
        {
            var result = _controller.SyncEmployerAccounts();

            Assert.That(result, Is.InstanceOf<NoContentResult>());
            _backgroundTaskQueueMock.Verify(q => q.QueueBackgroundRequest(
                It.IsAny<SyncEmployerAccountsCommand>(),
                It.Is<string>(s => s == "Sync employer accounts"),
                It.IsAny<Action<object, TimeSpan, ILogger<TaskQueueHostedService>>>()), Times.Once);
        }

        [Test]
        public void SyncEmployerAccounts_ReturnsInternalServerError_AndLogs_WhenExceptionThrown()
        {
            _backgroundTaskQueueMock.Setup(q => q.QueueBackgroundRequest(
                It.IsAny<SyncEmployerAccountsCommand>(),
                It.IsAny<string>(),
                It.IsAny<Action<object, TimeSpan, ILogger<TaskQueueHostedService>>>()))
                .Throws(new Exception("fail"));

            var result = _controller.SyncEmployerAccounts();

            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusResult = result as StatusCodeResult;
            Assert.That(statusResult.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error syncing employer accounts.")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public async Task GetAccountsBatch_ReturnsOk_WhenAccountIdsAreNotNull()
        {
            var batchSize = 10;
            var accountIds = new List<long> { 1, 2, 3, 4, 5 };
            var resultObj = new GetAccountsBatchResult { AccountIds = accountIds };
            _mediatorMock.Setup(m => m.Send(It.Is<GetAccountsBatchQuery>(q => q.BatchSize == batchSize), CancellationToken.None)).ReturnsAsync(resultObj);

            var result = await _controller.GetAccountsBatch(batchSize);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            var response = okResult.Value;
            Assert.That(response, Is.Not.Null);
            var accountIdsProperty = response.GetType().GetProperty("AccountIds");
            Assert.That(accountIdsProperty, Is.Not.Null);
            Assert.That(accountIdsProperty.GetValue(response), Is.EqualTo(accountIds));
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetAccountsBatchQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetAccountsBatch_ReturnsNotFound_WhenAccountIdsAreNull()
        {
            var batchSize = 10;
            var resultObj = new GetAccountsBatchResult { AccountIds = null };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAccountsBatchQuery>(), CancellationToken.None)).ReturnsAsync(resultObj);

            var result = await _controller.GetAccountsBatch(batchSize);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetAccountsBatchQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetAccountsBatch_ReturnsNotFound_WhenResultIsNull()
        {
            var batchSize = 10;
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAccountsBatchQuery>(), CancellationToken.None)).ReturnsAsync((GetAccountsBatchResult)null);

            var result = await _controller.GetAccountsBatch(batchSize);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetAccountsBatchQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetAccountsBatch_ReturnsInternalServerError_AndLogs_WhenExceptionThrown()
        {
            var batchSize = 10;
            var boom = new InvalidOperationException("test");
            _mediatorMock
                .Setup(m => m.Send(It.Is<GetAccountsBatchQuery>(q => q.BatchSize == batchSize), CancellationToken.None))
                .ThrowsAsync(boom);

            var result = await _controller.GetAccountsBatch(batchSize);

            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusResult = result as StatusCodeResult;
            Assert.That(statusResult.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));

            _loggerMock.VerifyLogErrorContains("Error getting accounts batch.", boom, Times.Once());
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetAccountsBatchQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpsertFeedbackTransaction_ReturnsNoContent_WhenSuccessful()
        {
            var accountId = 12345L;
            _mediatorMock.Setup(m => m.Send(It.Is<UpsertFeedbackTransactionCommand>(c => c.AccountId == accountId), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.UpsertFeedbackTransaction(accountId);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
            _mediatorMock.Verify(m => m.Send(It.Is<UpsertFeedbackTransactionCommand>(c => c.AccountId == accountId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpsertFeedbackTransaction_ReturnsInternalServerError_WhenExceptionThrown()
        {
            var accountId = 12345L;
            var exception = new InvalidOperationException("Test exception");
            _mediatorMock.Setup(m => m.Send(It.Is<UpsertFeedbackTransactionCommand>(c => c.AccountId == accountId), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            var result = await _controller.UpsertFeedbackTransaction(accountId);

            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusResult = result as StatusCodeResult;
            Assert.That(statusResult.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
            _mediatorMock.Verify(m => m.Send(It.Is<UpsertFeedbackTransactionCommand>(c => c.AccountId == accountId), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
