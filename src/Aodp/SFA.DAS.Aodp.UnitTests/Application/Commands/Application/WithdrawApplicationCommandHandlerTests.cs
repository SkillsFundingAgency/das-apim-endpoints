using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Aodp.Application;
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
    public class WithdrawApplicationCommandHandlerTests
    {
        private static readonly Guid ApplicationId = Guid.NewGuid();
        private const string WithdrawnBy = "Test User";
        private const string WithdrawnByEmail = "user@test.com";
        private const string TemplateName = EmailTemplateNames.QFASTSubmittedApplicationChangedNotification;
        private const NotificationRecipientKind RecipientKind = NotificationRecipientKind.QfauMailbox;
        private const string ExceptionMessage = "api exception";
        private const string GenericErrorMessage = "There was a problem withdrawing the application.";

        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClient = null!;
        private Mock<IEmailService> _emailService = null!;
        private Mock<ILogger<WithdrawApplicationCommandHandler>> _logger = null!;
        private WithdrawApplicationCommandHandler _handler = null!;

        [SetUp]
        public void SetUp()
        {
            _apiClient = new Mock<IAodpApiClient<AodpApiConfiguration>>();
            _emailService = new Mock<IEmailService>();
            _logger = new Mock<ILogger<WithdrawApplicationCommandHandler>>();

            _handler = new WithdrawApplicationCommandHandler(
                _apiClient.Object,
                _emailService.Object,
                _logger.Object);
        }

        [Test]
        public async Task Handle_ValidResponse_SendsEmail_AndReturnsSuccess()
        {
            var notifications = new List<NotificationDefinition>
            {
                new() { TemplateName = TemplateName, RecipientKind = RecipientKind }
            };

            var innerResponse = new WithdrawApplicationCommandResponse
            {
                Notifications = notifications
            };

            var apiResponse = new ApiResponse<WithdrawApplicationCommandResponse>(
                innerResponse,
                HttpStatusCode.OK,
                string.Empty);

            _apiClient
                .Setup(c => c.PostWithResponseCode<WithdrawApplicationCommandResponse>(
                    It.IsAny<WithdrawApplicationApiRequest>(), true))
                .ReturnsAsync(apiResponse);

            var request = new WithdrawApplicationCommand
            {
                ApplicationId = ApplicationId,
                WithdrawnBy = WithdrawnBy,
                WithdrawnByEmail = WithdrawnByEmail
            };

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Success, Is.True);
                Assert.That(result.ErrorMessage, Is.Null.Or.Empty);

                _apiClient.Verify(c =>
                    c.PostWithResponseCode<WithdrawApplicationCommandResponse>(
                        It.Is<WithdrawApplicationApiRequest>(r =>
                            r.ApplicationId == ApplicationId &&
                            r.Data == request), true),
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
            var innerResponse = new WithdrawApplicationCommandResponse
            {
                Notifications = new List<NotificationDefinition>()
            };

            var apiResponse = new ApiResponse<WithdrawApplicationCommandResponse>(
                innerResponse,
                HttpStatusCode.OK,
                string.Empty);

            _apiClient
                .Setup(c => c.PostWithResponseCode<WithdrawApplicationCommandResponse>(
                    It.IsAny<WithdrawApplicationApiRequest>(), true))
                .ReturnsAsync(apiResponse);

            var request = new WithdrawApplicationCommand
            {
                ApplicationId = ApplicationId,
                WithdrawnBy = WithdrawnBy,
                WithdrawnByEmail = WithdrawnByEmail
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
        public async Task Handle_ApiClientThrows_ReturnsGenericError_AndDoesNotSendEmail()
        {
            _apiClient
                .Setup(c => c.PostWithResponseCode<WithdrawApplicationCommandResponse>(
                    It.IsAny<WithdrawApplicationApiRequest>(), true))
                .ThrowsAsync(new InvalidOperationException(ExceptionMessage));

            var request = new WithdrawApplicationCommand
            {
                ApplicationId = ApplicationId,
                WithdrawnBy = WithdrawnBy,
                WithdrawnByEmail = WithdrawnByEmail
            };

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo(GenericErrorMessage));

                _emailService.Verify(
                    s => s.SendAsync(It.IsAny<IReadOnlyCollection<NotificationDefinition>>(), default),
                    Times.Never);
            });
        }
    }
}
