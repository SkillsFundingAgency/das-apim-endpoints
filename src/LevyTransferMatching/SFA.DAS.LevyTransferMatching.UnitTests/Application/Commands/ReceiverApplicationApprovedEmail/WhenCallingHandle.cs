﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationOutcome;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.ReceiverApplicationApprovedEmail
{
    public class WhenCallingHandle
    {
        private ReceiverApplicationApprovedEmailCommandHandler _handler;
        private Mock<IAccountsService> _accountsService;
        private Mock<ILogger<ReceiverApplicationApprovedEmailCommandHandler>> _logger;
        private Mock<INotificationService> _notificationService;
        private IFixture _fixture;
        private ReceiverApplicationApprovedEmailCommand _command;
        private List<TeamMember> _accountUsers;
        private Account _account;

        [SetUp]
        public void SetUp()
        {
            _accountsService = new Mock<IAccountsService>();
            _logger = new Mock<ILogger<ReceiverApplicationApprovedEmailCommandHandler>>();
            _notificationService = new Mock<INotificationService>();
            _fixture = new Fixture();

            _command = _fixture.Create<ReceiverApplicationApprovedEmailCommand>();

            _accountUsers = _fixture.CreateMany<TeamMember>(1).ToList();
            _accountUsers[0].Role = "Owner";
            _accountUsers[0].CanReceiveNotifications = true;

            _account = _fixture.Create<Account>();

            _accountsService
                .Setup(x => x.GetAccountUsers(It.IsAny<long>()))
                .ReturnsAsync(_accountUsers);

            _accountsService
                .Setup(x => x.GetAccount(It.IsAny<string>()))
                .ReturnsAsync(_account);

            _handler = new ReceiverApplicationApprovedEmailCommandHandler(
                _logger.Object,
                _notificationService.Object,
                _accountsService.Object);
        }

        [Test]
        public async Task Handle_SendsEmail_WhenReceiverApplicationApproved()
        {
            await _handler.Handle(_command, CancellationToken.None);
            _accountsService.Verify(x => x.GetAccountUsers(It.IsAny<long>()), Times.Once);
            _accountsService.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);

            foreach (var user in _accountUsers)
            {
                _notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Once);
            }
        }
    }
}
