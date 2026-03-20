using Moq;
using SFA.DAS.Aodp.Application.Commands.Application.Application;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Models;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.Aodp.Application.Tests.Commands.Application.Application
{
    [TestFixture]
    public class SubmitApplicationCommandHandlerOuterTests
    {
        private static readonly Guid ApplicationId = Guid.NewGuid();
        private const string SubmittedBy = "Test User";
        private const string SubmittedByEmail = "user@test.com";
        private const string TemplateName = EmailTemplateNames.QFASTSubmittedApplicationChangedNotification;
        private const NotificationRecipientKind RecipientKind = NotificationRecipientKind.QfauMailbox;
        private const string ExceptionMessage = "api exception";

        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClient = null!;
        private Mock<IEmailService> _emailService = null!;
        private SubmitApplicationCommandHandler _handler = null!;

        [SetUp]
        public void SetUp()
        {
            _apiClient = new Mock<IAodpApiClient<AodpApiConfiguration>>();
            _emailService = new Mock<IEmailService>();

            _handler = new SubmitApplicationCommandHandler(_apiClient.Object, _emailService.Object);
        }

        [Test]
        public async Task Handle_ValidResponse_SendsEmail_AndReturnsSuccess()
        {
            var notifications = new List<NotificationDefinition>
            {
                new() { TemplateName = TemplateName, RecipientKind = RecipientKind }
            };

            var innerResponse = new SubmitApplicationCommandResponse
            {
                Notifications = notifications
            };

            var apiResponse = new ApiResponse<SubmitApplicationCommandResponse>(
                innerResponse,
                HttpStatusCode.OK,
                string.Empty);

            _apiClient
                .Setup(c => c.PutWithResponseCode<SubmitApplicationCommandResponse>(
                    It.IsAny<SubmitApplicationApiRequest>()))
                .ReturnsAsync(apiResponse);

            var request = new SubmitApplicationCommand
            {
                ApplicationId = ApplicationId,
                SubmittedBy = SubmittedBy,
                SubmittedByEmail = SubmittedByEmail
            };

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Success, Is.True);
                Assert.That(result.ErrorMessage, Is.Null.Or.Empty);

                _apiClient.Verify(c =>
                    c.PutWithResponseCode<SubmitApplicationCommandResponse>(
                        It.Is<SubmitApplicationApiRequest>(r =>
                            r.ApplicationId == ApplicationId &&
                            r.Data == request)),
                    Times.Once);

                _emailService.Verify(s => s.SendAsync(
                    It.Is<IReadOnlyCollection<NotificationDefinition>>(n =>
                        n.Count == 1 &&
                        n.First().TemplateName == TemplateName &&
                        n.First().RecipientKind == RecipientKind),
                    default), Times.Once);
            });
        }

        [Test]
        public async Task Handle_NoNotifications_StillCallsEmailService_AndReturnsSuccess()
        {
            var innerResponse = new SubmitApplicationCommandResponse
            {
                Notifications = new List<NotificationDefinition>()
            };

            var apiResponse = new ApiResponse<SubmitApplicationCommandResponse>(
                innerResponse,
                HttpStatusCode.OK,
                string.Empty);

            _apiClient
                .Setup(c => c.PutWithResponseCode<SubmitApplicationCommandResponse>(
                    It.IsAny<SubmitApplicationApiRequest>()))
                .ReturnsAsync(apiResponse);

            var request = new SubmitApplicationCommand
            {
                ApplicationId = ApplicationId,
                SubmittedBy = SubmittedBy,
                SubmittedByEmail = SubmittedByEmail
            };

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Success, Is.True);
                Assert.That(result.ErrorMessage, Is.Null.Or.Empty);

                _emailService.Verify(
                    s => s.SendAsync(
                        It.Is<IReadOnlyCollection<NotificationDefinition>>(n => n != null && !n.Any()),
                        default),
                    Times.Once);
            });
        }

        [Test]
        public async Task Handle_ApiClientThrows_ReturnsError_AndDoesNotSendEmail()
        {
            _apiClient
                .Setup(c => c.PutWithResponseCode<SubmitApplicationCommandResponse>(
                    It.IsAny<SubmitApplicationApiRequest>()))
                .ThrowsAsync(new InvalidOperationException(ExceptionMessage));

            var request = new SubmitApplicationCommand
            {
                ApplicationId = ApplicationId,
                SubmittedBy = SubmittedBy,
                SubmittedByEmail = SubmittedByEmail
            };

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo(ExceptionMessage));

                _emailService.Verify(
                    s => s.SendAsync(It.IsAny<IReadOnlyCollection<NotificationDefinition>>(), default),
                    Times.Never);
            });
        }
    }
}
