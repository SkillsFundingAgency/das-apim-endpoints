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
using SFA.DAS.EmployerFeedback.Application.Commands.SyncEmployerAccounts;

namespace SFA.DAS.EmployerFeedback.Api.UnitTests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<AccountController>> _loggerMock;
        private AccountController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<AccountController>>();
            _controller = new AccountController(_loggerMock.Object, _mediatorMock.Object);
        }

        [Test]
        public async Task UpdateEmployerAccounts_ReturnsNoContent_WhenCommandSucceeds()
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SyncEmployerAccountsCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _controller.UpdateEmployerAccounts();

            Assert.That(result, Is.InstanceOf<NoContentResult>());
            _mediatorMock.Verify(m => m.Send(It.IsAny<SyncEmployerAccountsCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UpdateEmployerAccounts_ReturnsInternalServerError_AndLogs_WhenExceptionThrown()
        {
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SyncEmployerAccountsCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("fail"));

            var result = await _controller.UpdateEmployerAccounts();

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
