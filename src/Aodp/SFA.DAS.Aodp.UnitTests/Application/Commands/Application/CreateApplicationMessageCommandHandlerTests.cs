using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SFA.DAS.Aodp.Application.Commands.Application.Application;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Application.Messages;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.Aodp.Application.Tests.Commands.Application
{
    [TestFixture]
    public class CreateApplicationMessageCommandHandlerOuterTests
    {
        private static readonly Guid ApplicationId = Guid.NewGuid();
        private static readonly Guid MessageId = Guid.NewGuid();
        private static readonly Guid TemplateId = Guid.NewGuid();

        private const string QfauMailboxEmail = "qfau@test.com";
        private const string QfastBaseUrl = "https://qfast-test.example.com";

        private const string TemplateName = EmailTemplateNames.QFASTApplicationSubmittedNotification;

        private const string QfauUserType = "Qfau";
        private const string ApplicationSubmittedMessageType = "ApplicationSubmitted";
        private const string InternalNotesMessageType = "InternalNotes";

        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClient = null!;
        private Mock<INotificationService> _notificationService = null!;
        private Mock<IOptions<AodpConfiguration>> _options = null!;
        private Mock<ILogger<CreateApplicationMessageCommandHandler>> _logger = null!;

        private CreateApplicationMessageCommandHandler _handler = null!;

        [SetUp]
        public void SetUp()
        {
            _apiClient = new Mock<IAodpApiClient<AodpApiConfiguration>>();
            _notificationService = new Mock<INotificationService>();
            _options = new Mock<IOptions<AodpConfiguration>>();
            _logger = new Mock<ILogger<CreateApplicationMessageCommandHandler>>();

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

            _handler = new CreateApplicationMessageCommandHandler(
                _apiClient.Object,
                _notificationService.Object,
                _options.Object,
                _logger.Object);
        }

        [Test]
        public async Task Handle_ValidResponse_SendsEmail_AndReturnsSuccess()
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

            var innerResponse = new CreateApplicationMessageCommandResponse
            {
                Id = MessageId,
                Notifications = notifications
            };

            var apiResponse = new ApiResponse<CreateApplicationMessageCommandResponse>(
                innerResponse,
                HttpStatusCode.OK,
                string.Empty);

            _apiClient
                .Setup(c => c.PostWithResponseCode<CreateApplicationMessageCommandResponse>(
                    It.IsAny<CreateApplicationMessageApiRequest>(), true))
                .ReturnsAsync(apiResponse);

            var request = new CreateApplicationMessageCommand
            {
                ApplicationId = ApplicationId,
                MessageType = ApplicationSubmittedMessageType,
                MessageText = "Test",
                UserType = QfauUserType,
                SentByEmail = "sender@test.com",
                SentByName = "Sender"
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Success, Is.True);
                Assert.That(result.Value, Is.Not.Null);
                Assert.That(result.Value.Id, Is.EqualTo(MessageId));

                _apiClient.Verify(c =>
                    c.PostWithResponseCode<CreateApplicationMessageCommandResponse>(
                        It.Is<CreateApplicationMessageApiRequest>(r =>
                            r.ApplicationId == ApplicationId &&
                            r.Data == request), true),
                    Times.Once);

                _notificationService.Verify(s =>
                    s.Send(It.Is<SendEmailCommand>(cmd =>
                        cmd.TemplateId == TemplateId.ToString() &&
                        cmd.RecipientsAddress == QfauMailboxEmail &&
                        cmd.Tokens != null &&
                        cmd.Tokens.ContainsKey("QFASTBaseUrl") &&
                        cmd.Tokens["QFASTBaseUrl"] == QfastBaseUrl)),
                    Times.Once);
            });
        }

        [Test]
        public async Task Handle_NoNotifications_DoesNotSendEmail_StillReturnsSuccess()
        {
            // Arrange
            var innerResponse = new CreateApplicationMessageCommandResponse
            {
                Id = MessageId,
                Notifications = new List<NotificationDefinition>()
            };

            var apiResponse = new ApiResponse<CreateApplicationMessageCommandResponse>(
                innerResponse,
                HttpStatusCode.OK,
                string.Empty);

            _apiClient
                .Setup(c => c.PostWithResponseCode<CreateApplicationMessageCommandResponse>(
                    It.IsAny<CreateApplicationMessageApiRequest>(), true))
                .ReturnsAsync(apiResponse);

            var request = new CreateApplicationMessageCommand
            {
                ApplicationId = ApplicationId,
                MessageType = InternalNotesMessageType,
                MessageText = "Test",
                UserType = QfauUserType
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Success, Is.True);
                Assert.That(result.Value, Is.Not.Null);
                Assert.That(result.Value.Id, Is.EqualTo(MessageId));

                _notificationService.Verify(
                    s => s.Send(It.IsAny<SendEmailCommand>()),
                    Times.Never);
            });
        }

        [Test]
        public async Task Handle_TemplateIdMissing_LogsWarning_DoesNotSendEmail()
        {
            // Arrange
            _options.SetupGet(o => o.Value).Returns(new AodpConfiguration
            {
                QfauReviewerEmailAddress = QfauMailboxEmail,
                QFASTBaseUrl = QfastBaseUrl,
                NotificationTemplates = new List<NotificationTemplate>()
            });

            var notifications = new List<NotificationDefinition>
            {
                new()
                {
                    TemplateName = TemplateName,
                    RecipientKind = NotificationRecipientKind.QfauMailbox
                }
            };

            var innerResponse = new CreateApplicationMessageCommandResponse
            {
                Id = MessageId,
                Notifications = notifications
            };

            var apiResponse = new ApiResponse<CreateApplicationMessageCommandResponse>(
                innerResponse,
                HttpStatusCode.OK,
                string.Empty);

            _apiClient
                .Setup(c => c.PostWithResponseCode<CreateApplicationMessageCommandResponse>(
                    It.IsAny<CreateApplicationMessageApiRequest>(), true))
                .ReturnsAsync(apiResponse);

            var request = new CreateApplicationMessageCommand
            {
                ApplicationId = ApplicationId,
                MessageType = ApplicationSubmittedMessageType,
                MessageText = "Test",
                UserType = QfauUserType
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Success, Is.True); 

                _notificationService.Verify(
                    s => s.Send(It.IsAny<SendEmailCommand>()),
                    Times.Never);
            });
        }

        [Test]
        public async Task Handle_ApiClientThrows_ReturnsError_DoesNotSendEmail()
        {
            // Arrange
            _apiClient
                .Setup(c => c.PostWithResponseCode<CreateApplicationMessageCommandResponse>(
                    It.IsAny<CreateApplicationMessageApiRequest>(), true))
                .ThrowsAsync(new InvalidOperationException("api exception"));

            var request = new CreateApplicationMessageCommand
            {
                ApplicationId = ApplicationId,
                MessageType = ApplicationSubmittedMessageType,
                MessageText = "Test",
                UserType = QfauUserType
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo("api exception"));

                _notificationService.Verify(
                    s => s.Send(It.IsAny<SendEmailCommand>()),
                    Times.Never);
            });
        }
    }
}
