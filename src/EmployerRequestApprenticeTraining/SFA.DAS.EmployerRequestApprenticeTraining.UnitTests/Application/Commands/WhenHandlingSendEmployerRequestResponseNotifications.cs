using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.SendResponseNotification;
using SFA.DAS.EmployerRequestApprenticeTraining.Configuration;
using SFA.DAS.EmployerRequestApprenticeTraining.Models;
using SFA.DAS.Encoding;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        
        [Test]
        public async Task Then_Notification_Is_Sent_If_Template_Found()
        {
            // Arrange
            var templateId = _mockOptions.Object.Value.NotificationTemplates
                .First(p => p.TemplateName == EmailTemplateNames.RATEmployerResponseNotification).TemplateId.ToString();

            var command = new SendResponseNotificationCommand 
            { 
                AccountId =  1005268,
                FirstName = "Test",
                EmailAddress = "receiver@gmail.com",
                ManageNotificationSettingsLink = "http://thesite/settings/notifications",
                ManageRequestsLink = "http://thesite/accounts/{0}/dashboard",
                RequestedBy = Guid.NewGuid(),
                Standards = new List<StandardDetails> 
                {
                    new StandardDetails{ StandardLevel = 1, StandardTitle = "Agricultural"},
                    new StandardDetails{ StandardLevel = 2, StandardTitle = "Banking"},
                    new StandardDetails{ StandardLevel = 3, StandardTitle = "Commerce"}
                }
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            var expectedCourseLevels = new StringBuilder();
            foreach (var course in command.Standards)
            {
                expectedCourseLevels.Append($"* {course.StandardTitle} (level{course.StandardLevel})\n");
            }

            _mockNotificationsService.Verify(n => n.Send(It.Is<SendEmailCommand>(c =>
                c.TemplateId == templateId &&
                c.RecipientsAddress == command.EmailAddress &&
                c.Tokens.GetValueOrDefault("user_name") == command.FirstName &&
                c.Tokens.GetValueOrDefault("course_level_bullet_points") == expectedCourseLevels.ToString() &&
                c.Tokens.GetValueOrDefault("dashboard_url") == string.Format(command.ManageRequestsLink, _encodedAccountId) &&
                c.Tokens.GetValueOrDefault("unsubscribe_url") == command.ManageNotificationSettingsLink

            )), Times.Once);



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
                    new StandardDetails { StandardTitle = "StandardTitle 1", StandardLevel = 1},
                    new StandardDetails { StandardTitle = "StandardTitle 2", StandardLevel = 2},
                }
            };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockNotificationsService.Verify(n => n.Send(It.IsAny<SendEmailCommand>()), Times.Never);
        }
    }
}
