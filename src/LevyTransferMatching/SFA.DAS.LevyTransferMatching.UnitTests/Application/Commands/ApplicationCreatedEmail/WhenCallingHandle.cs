using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using Microsoft.Extensions.Logging;
using System.Linq;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.ApplicationCreatedEmail
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private ApplicationCreatedEmailCommandHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private Mock<IAccountsService> _accountsService;
        private Mock<ILogger<ApplicationCreatedEmailCommandHandler>> _logger;
        private Mock<INotificationService> _notificationService;
        private IFixture _fixture;
        private ApplicationCreatedEmailCommand _command;
        private GetApplicationResponse _application;
        private List<TeamMember> _accountUsers;

        [SetUp]
        public void SetUp()
        {
            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _accountsService = new Mock<IAccountsService>();
            _logger = new Mock<ILogger<ApplicationCreatedEmailCommandHandler>>();
            _notificationService = new Mock<INotificationService>();
            _fixture = new Fixture();

            _command = _fixture.Create<ApplicationCreatedEmailCommand>();
            _application = _fixture.Create<GetApplicationResponse>();
            
            _accountUsers = _fixture.CreateMany<TeamMember>(1).ToList();
            _accountUsers[0].Role = "Owner";
            _accountUsers[0].CanReceiveNotifications = true;

            _levyTransferMatchingService
                .Setup(x => x.GetApplication(It.IsAny<GetApplicationRequest>()))
                .ReturnsAsync(_application);

            _accountsService
                .Setup(x => x.GetAccountUsers(It.IsAny<long>()))
                .ReturnsAsync(_accountUsers);

            _handler = new ApplicationCreatedEmailCommandHandler(
                _levyTransferMatchingService.Object,
                _logger.Object,
                _notificationService.Object,
                _accountsService.Object);
        }
       
        [Test]
        public async Task Handle_SendsEmailWithCorrectTemplate_Match_Percentage_Less_than_100()
        {
            // Arrange          
            _application.MatchPercentage = 75;

            // Act
            await _handler.Handle(_command, CancellationToken.None);

            // Assert
            foreach (var user in _accountUsers)
            {
                _notificationService.Verify(x => x.Send(It.Is<SendEmailCommand>(cmd =>
                         cmd.TemplateId == "PartialMatchApplicationCreated")), Times.Once);
            }
        }

        [Test]
        public async Task Handle_SendsEmailWithCorrectTemplate_ApprovalOption_Is_Delayed()
        {
            // Arrange          
            _application.MatchPercentage = 100;
            _application.AutomaticApprovalOption = AutomaticApprovalOption.DelayedAutoApproval;

            // Act
            await _handler.Handle(_command, CancellationToken.None);

            // Assert
            foreach (var user in _accountUsers)
            {
                _notificationService.Verify(x => x.Send(It.Is<SendEmailCommand>(cmd =>
                        cmd.TemplateId == "ApplicationCreatedForDelayedPledge")), Times.Once);
            }
        }
    }
}
