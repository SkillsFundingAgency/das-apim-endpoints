using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.RejectApplicationEmail
{
    public  class WhenCallingHandle
    {
        private ApplicationRejectedEmailCommandHandler _handler;
        private Mock<IAccountsService> _accountsService;
        private Mock<ILogger<ApplicationRejectedEmailCommandHandler>> _logger;
        private Mock<INotificationService> _notificationService;
        private IFixture _fixture;
        private ApplicationRejectedEmailCommand _command;
        private List<TeamMember> _accountUsers;

        [SetUp]
        public void SetUp()
        {
            _accountsService = new Mock<IAccountsService>();
            _logger = new Mock<ILogger<ApplicationRejectedEmailCommandHandler>>();
            _notificationService = new Mock<INotificationService>();
            _fixture = new Fixture();

            _command = _fixture.Create<ApplicationRejectedEmailCommand>();

            _accountUsers = _fixture.CreateMany<TeamMember>(1).ToList();
            _accountUsers[0].Role = "Owner";
            _accountUsers[0].CanReceiveNotifications = true;

            _accountsService
                .Setup(x => x.GetAccountUsers(It.IsAny<long>()))
                .ReturnsAsync(_accountUsers);


            _handler = new ApplicationRejectedEmailCommandHandler(
                _logger.Object,
                _notificationService.Object,
                _accountsService.Object);
        }


        [Test]
        public async Task Handle_SendsEmail_WhenApplicationRejected()
        {
            await _handler.Handle(_command, CancellationToken.None);
            _accountsService.Verify(x => x.GetAccountUsers(It.IsAny<long>()), Times.Once);

            foreach (var user in _accountUsers)
            {
                _notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Once);
            }
        }
    }
}
