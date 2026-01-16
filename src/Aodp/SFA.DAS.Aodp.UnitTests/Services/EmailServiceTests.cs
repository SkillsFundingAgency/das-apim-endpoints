using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Models;
using SFA.DAS.Aodp.Services;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Application.Tests.Services
{
    [TestFixture]
    public class EmailServiceTests
    {
        private static readonly Guid TemplateId = Guid.NewGuid();

        private const string QfauMailboxEmail = "qfau@test.com";
        private const string QfastBaseUrl = "https://qfast-test.example.com";
        private const string TemplateName = EmailTemplateNames.QFASTApplicationSubmittedNotification;

        private Mock<INotificationService> _notificationService = null!;
        private Mock<IOptions<AodpConfiguration>> _options = null!;
        private Mock<ILogger<EmailService>> _logger = null!;

        private EmailService _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _notificationService = new Mock<INotificationService>();
            _options = new Mock<IOptions<AodpConfiguration>>();
            _logger = new Mock<ILogger<EmailService>>();

            var config = new AodpConfiguration
            {
                QfauReviewerEmailAddress = QfauMailboxEmail,
                QFASTBaseUrl = QfastBaseUrl,
                NotificationTemplates = new List<NotificationTemplate>
                {
                    new NotificationTemplate
                    {
                        TemplateName = TemplateName,
                        TemplateId = TemplateId
                    }
                }
            };

            _options.SetupGet(o => o.Value).Returns(config);

            _sut = new EmailService(_notificationService.Object, _options.Object, _logger.Object);
        }

        [Test]
        public async Task SendAsync_ValidQfauNotification_SendsEmailWithExpectedValues()
        {
            // Arrange
            var notifications = new List<NotificationDefinition>
            {
                new()
                {
                    TemplateName = TemplateName,
                    RecipientKind = NotificationRecipientKind.QfauMailbox
                }
            };

            // Act
            await _sut.SendAsync(notifications, CancellationToken.None);

            // Assert
            _notificationService.Verify(
                s => s.Send(It.Is<SendEmailCommand>(cmd =>
                        cmd.TemplateId == TemplateId.ToString()
                        && cmd.RecipientsAddress == QfauMailboxEmail
                        && cmd.Tokens != null
                        && cmd.Tokens.ContainsKey("QFASTBaseUrl")
                        && cmd.Tokens["QFASTBaseUrl"] == QfastBaseUrl)),
                Times.Once);
        }

        [Test]
        public async Task SendAsync_TemplateIdMissing_DoesNotSendEmail()
        {
            _options.SetupGet(o => o.Value).Returns(new AodpConfiguration
            {
                QfauReviewerEmailAddress = QfauMailboxEmail,
                QFASTBaseUrl = QfastBaseUrl,
                NotificationTemplates = new List<NotificationTemplate>() // no TemplateName entry
            });

            _sut = new EmailService(_notificationService.Object, _options.Object, _logger.Object);

            var notifications = new List<NotificationDefinition>
            {
                new()
                {
                    TemplateName = TemplateName, 
                    RecipientKind = NotificationRecipientKind.QfauMailbox
                }
            };

            // Act
            await _sut.SendAsync(notifications, CancellationToken.None);

            // Assert
            _notificationService.Verify(
                s => s.Send(It.IsAny<SendEmailCommand>()),
                Times.Never);
        }

        [Test]
        public async Task SendAsync_NoNotifications_DoesNothing()
        {
            // Arrange
            var notifications = new List<NotificationDefinition>();

            // Act
            await _sut.SendAsync(notifications, CancellationToken.None);

            // Assert
            _notificationService.Verify(
                s => s.Send(It.IsAny<SendEmailCommand>()),
                Times.Never);
        }

        [Test]
        public async Task SendAsync_DirectEmail_UsesNotificationEmailAddress()
        {
            // Arrange
            const string directEmail = "direct@test.com";

            var notifications = new List<NotificationDefinition>
            {
                new()
                {
                    TemplateName = TemplateName,
                    RecipientKind = NotificationRecipientKind.DirectEmail,
                    EmailAddress = directEmail
                }
            };

            // Act
            await _sut.SendAsync(notifications, CancellationToken.None);

            // Assert
            _notificationService.Verify(
                s => s.Send(It.Is<SendEmailCommand>(cmd =>
                        cmd.TemplateId == TemplateId.ToString()
                        && cmd.RecipientsAddress == directEmail)),
                Times.Once);
        }
    }
}
