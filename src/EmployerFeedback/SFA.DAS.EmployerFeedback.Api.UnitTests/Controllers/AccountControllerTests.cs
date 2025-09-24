using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Api.Controllers;
using SFA.DAS.EmployerFeedback.Api.TaskQueue;
using SFA.DAS.EmployerFeedback.Application.Commands.SyncEmployerAccounts;

namespace SFA.DAS.EmployerFeedback.Api.UnitTests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Mock<IBackgroundTaskQueue> _backgroundTaskQueueMock;
        private Mock<ILogger<AccountController>> _loggerMock;
        private AccountController _controller;

        [SetUp]
        public void SetUp()
        {
            _backgroundTaskQueueMock = new Mock<IBackgroundTaskQueue>();
            _loggerMock = new Mock<ILogger<AccountController>>();
            _controller = new AccountController(_loggerMock.Object, _backgroundTaskQueueMock.Object);
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
    }
}
