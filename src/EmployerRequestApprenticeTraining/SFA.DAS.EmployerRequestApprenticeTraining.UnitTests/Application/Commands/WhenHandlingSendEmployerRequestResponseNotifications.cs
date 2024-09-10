using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.SendResponseNotification;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Models;
using SFA.DAS.EmployerRequestApprenticeTraining.Configuration;
using SFA.DAS.Encoding;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Commands
{
    public class WhenHandlingSendEmployerRequestResponseNotifications
    {
        private Mock<IEncodingService> _mockEncodingService;
        private Mock<INotificationService> _mockNotificationsService;
        private Mock<IOptions<EmployerRequestApprenticeTrainingConfiguration>> _mockOptions;
        private SendResponseNotificationCommandHandler _handler;
        private readonly string _encodedAccountId = "ABCDE";

        [SetUp]
        public void Arrange()
        {
            _mockNotificationsService = new Mock<INotificationService>();
            
            _mockEncodingService = new Mock<IEncodingService>();
            _mockEncodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.AccountId)).Returns(_encodedAccountId);

            _mockOptions = new Mock<IOptions<EmployerRequestApprenticeTrainingConfiguration>>();

            var config = new EmployerRequestApprenticeTrainingConfiguration
            {
                NotificationTemplates = new List<NotificationTemplate>
                {
                    new NotificationTemplate
                    {
                        TemplateName = EmailTemplateNames.RATEmployerResponseNotification,
                        TemplateId = Guid.NewGuid()
                    }
                }
            };
            _mockOptions.Setup(o => o.Value).Returns(config);

            _handler = new SendResponseNotificationCommandHandler( 
                _mockNotificationsService.Object, 
                _mockEncodingService.Object,
                _mockOptions.Object);        
        }
        
        [Test, MoqAutoData]
        public async Task Then_Notification_Is_Sent_If_Template_Found(SendResponseNotificationCommand command)
        {
            // Arrange
            var templateId = _mockOptions.Object.Value.NotificationTemplates.First().TemplateId.ToString();

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockNotificationsService.Verify(n => n.Send(It.Is<SendEmailCommand>(c => c.TemplateId == templateId && c.RecipientsAddress == command.EmailAddress)), Times.Once);
        }


        [Test, MoqAutoData]
        public async Task Then_Notification_Is_Not_Sent_If_Template_Not_Found()
        {
            // Arrange
            var emptyNotificationTemplates = new EmployerRequestApprenticeTrainingConfiguration
            {
                NotificationTemplates = new List<NotificationTemplate>()
            };
            _mockOptions.Setup(o => o.Value).Returns(emptyNotificationTemplates);

            SendResponseNotificationCommand command = new SendResponseNotificationCommand
            {
                AccountId = 123456,
                RequestedBy = Guid.NewGuid(),
                Standards = new List<StandardDetails>
                {
                    new StandardDetails { StandardTitle = "Title 1", StandardLevel = 1},
                    new StandardDetails { StandardTitle = "Title 2", StandardLevel = 2},
                }
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockNotificationsService.Verify(n => n.Send(It.IsAny<SendEmailCommand>()), Times.Never);
        }
    }
}
