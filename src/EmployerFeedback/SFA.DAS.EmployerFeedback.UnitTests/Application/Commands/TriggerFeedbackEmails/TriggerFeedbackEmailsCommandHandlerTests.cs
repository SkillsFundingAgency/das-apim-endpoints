using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Commands.TriggerFeedbackEmails;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;
using SFA.DAS.EmployerFeedback.Models;
using SFA.DAS.Encoding;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Commands.TriggerFeedbackEmails
{
    [TestFixture]
    public class TriggerFeedbackEmailsCommandHandlerTests
    {
        private Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> _mockEmployerFeedbackApiClient;
        private Mock<IAccountsApiClient<AccountsConfiguration>> _mockAccountsApiClient;
        private Mock<INotificationService> _mockNotificationService;
        private Mock<ILogger<TriggerFeedbackEmailsCommandHandler>> _mockLogger;
        private Mock<IEncodingService> _mockEncodingService;
        private TriggerFeedbackEmailsCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockEmployerFeedbackApiClient = new Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>>();
            _mockAccountsApiClient = new Mock<IAccountsApiClient<AccountsConfiguration>>();
            _mockNotificationService = new Mock<INotificationService>();
            _mockLogger = new Mock<ILogger<TriggerFeedbackEmailsCommandHandler>>();
            _mockEncodingService = new Mock<IEncodingService>();

            _handler = new TriggerFeedbackEmailsCommandHandler(
                _mockEmployerFeedbackApiClient.Object,
                _mockAccountsApiClient.Object,
                _mockNotificationService.Object,
                _mockLogger.Object,
                _mockEncodingService.Object);
        }

        [Test]
        public async Task Then_ProcessesValidFeedbackTransaction_AndSendsEmails()
        {
            var feedbackTransactionId = 12345L;
            var templateId = Guid.NewGuid();
            var hashedAccountId = "ABC123";

            var request = new TriggerFeedbackEmailsRequest
            {
                NotificationTemplates = new List<NotificationTemplateRequest>
                {
                    new NotificationTemplateRequest
                    {
                        TemplateName = "TestTemplate",
                        TemplateId = templateId
                    }
                },
                EmployerAccountsBaseUrl = "https://employer-accounts.test"
            };

            var feedbackTransaction = new GetFeedbackTransactionResponse
            {
                Id = feedbackTransactionId,
                AccountId = 999,
                AccountName = "Test Company",
                TemplateName = "TestTemplate",
                SendAfter = DateTime.UtcNow.AddDays(-1),
                SentDate = null
            };

            var eligibleUsers = new List<AccountUser>
            {
                new AccountUser
                {
                    Email = "user1@test.com",
                    FirstName = "User1",
                    CanReceiveNotifications = true,
                    Status = 2
                },
                new AccountUser
                {
                    Email = "user2@test.com",
                    FirstName = "User2",
                    CanReceiveNotifications = true,
                    Status = 2
                }
            };

            var command = new TriggerFeedbackEmailsCommand(feedbackTransactionId, request);

            _mockEmployerFeedbackApiClient
                .Setup(x => x.GetWithResponseCode<GetFeedbackTransactionResponse>(It.IsAny<GetFeedbackTransactionRequest>()))
                .ReturnsAsync(new ApiResponse<GetFeedbackTransactionResponse>(feedbackTransaction, HttpStatusCode.OK, string.Empty));

            _mockAccountsApiClient
                .Setup(x => x.GetWithResponseCode<GetAccountUsersResponse>(It.IsAny<GetAccountUsersRequest>()))
                .ReturnsAsync(new ApiResponse<GetAccountUsersResponse>(new GetAccountUsersResponse { eligibleUsers[0], eligibleUsers[1] }, HttpStatusCode.OK, string.Empty));

            _mockEncodingService
                .Setup(x => x.Encode(feedbackTransaction.AccountId, EncodingType.AccountId))
                .Returns(hashedAccountId);

            await _handler.Handle(command, CancellationToken.None);

            _mockEmployerFeedbackApiClient.Verify(x => x.GetWithResponseCode<GetFeedbackTransactionResponse>(
                It.Is<GetFeedbackTransactionRequest>(r => r.GetUrl.Contains(feedbackTransactionId.ToString()))), Times.Once);

            _mockAccountsApiClient.Verify(x => x.GetWithResponseCode<GetAccountUsersResponse>(
                It.Is<GetAccountUsersRequest>(r => r.GetUrl.Contains(feedbackTransaction.AccountId.ToString()))), Times.Once);

            _mockNotificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Exactly(eligibleUsers.Count));

            _mockEmployerFeedbackApiClient.Verify(x => x.Put<UpdateFeedbackTransactionData>(
                It.Is<UpdateFeedbackTransactionRequest>(r =>
                    r.Data.TemplateId == templateId &&
                    r.Data.SentCount == eligibleUsers.Count)), Times.Once);
        }

        [Test]
        public void Then_ThrowsException_WhenFeedbackTransactionNotFound()
        {
            var feedbackTransactionId = 12345L;
            var request = new TriggerFeedbackEmailsRequest
            {
                NotificationTemplates = new List<NotificationTemplateRequest>(),
                EmployerAccountsBaseUrl = "https://employer-accounts.test"
            };
            var command = new TriggerFeedbackEmailsCommand(feedbackTransactionId, request);

            _mockEmployerFeedbackApiClient
                .Setup(x => x.GetWithResponseCode<GetFeedbackTransactionResponse>(It.IsAny<GetFeedbackTransactionRequest>()))
                .ReturnsAsync(new ApiResponse<GetFeedbackTransactionResponse>(null, HttpStatusCode.NotFound, string.Empty));

            var exception = Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.That(exception.Message, Does.Contain($"Feedback transaction {feedbackTransactionId} not found"));
        }

        [Test]
        public void Then_ThrowsException_WhenSendAfterDateIsInFuture()
        {
            var feedbackTransactionId = 12345L;
            var request = new TriggerFeedbackEmailsRequest
            {
                NotificationTemplates = new List<NotificationTemplateRequest>(),
                EmployerAccountsBaseUrl = "https://employer-accounts.test"
            };

            var feedbackTransaction = new GetFeedbackTransactionResponse
            {
                Id = feedbackTransactionId,
                AccountId = 999,
                AccountName = "Test Company",
                TemplateName = "TestTemplate",
                SendAfter = DateTime.UtcNow.AddDays(1),
                SentDate = null
            };

            var command = new TriggerFeedbackEmailsCommand(feedbackTransactionId, request);

            _mockEmployerFeedbackApiClient
                .Setup(x => x.GetWithResponseCode<GetFeedbackTransactionResponse>(It.IsAny<GetFeedbackTransactionRequest>()))
                .ReturnsAsync(new ApiResponse<GetFeedbackTransactionResponse>(feedbackTransaction, HttpStatusCode.OK, string.Empty));

            var exception = Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.That(exception.Message, Is.EqualTo("Feedback transaction sendAfter date is in the future"));
        }

        [Test]
        public async Task Then_ExitsEarly_WhenFeedbackTransactionAlreadySent()
        {
            var feedbackTransactionId = 12345L;
            var request = new TriggerFeedbackEmailsRequest
            {
                NotificationTemplates = new List<NotificationTemplateRequest>(),
                EmployerAccountsBaseUrl = "https://employer-accounts.test"
            };

            var feedbackTransaction = new GetFeedbackTransactionResponse
            {
                Id = feedbackTransactionId,
                AccountId = 999,
                AccountName = "Test Company",
                TemplateName = "TestTemplate",
                SendAfter = DateTime.UtcNow.AddDays(-2),
                SentDate = DateTime.UtcNow.AddDays(-1)
            };

            var command = new TriggerFeedbackEmailsCommand(feedbackTransactionId, request);

            _mockEmployerFeedbackApiClient
                .Setup(x => x.GetWithResponseCode<GetFeedbackTransactionResponse>(It.IsAny<GetFeedbackTransactionRequest>()))
                .ReturnsAsync(new ApiResponse<GetFeedbackTransactionResponse>(feedbackTransaction, HttpStatusCode.OK, string.Empty));

            await _handler.Handle(command, CancellationToken.None);

            _mockAccountsApiClient.Verify(x => x.GetWithResponseCode<GetAccountUsersResponse>(It.IsAny<GetAccountUsersRequest>()), Times.Never);
            _mockNotificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Never);
        }

        [Test]
        public void Then_ThrowsException_WhenNoMatchingTemplateFound()
        {
            var feedbackTransactionId = 12345L;
            var request = new TriggerFeedbackEmailsRequest
            {
                NotificationTemplates = new List<NotificationTemplateRequest>
                {
                    new NotificationTemplateRequest
                    {
                        TemplateName = "DifferentTemplate",
                        TemplateId = Guid.NewGuid()
                    }
                },
                EmployerAccountsBaseUrl = "https://employer-accounts.test"
            };

            var feedbackTransaction = new GetFeedbackTransactionResponse
            {
                Id = feedbackTransactionId,
                AccountId = 999,
                AccountName = "Test Company",
                TemplateName = "NonExistentTemplate",
                SendAfter = DateTime.UtcNow.AddDays(-1),
                SentDate = null
            };

            var command = new TriggerFeedbackEmailsCommand(feedbackTransactionId, request);

            _mockEmployerFeedbackApiClient
                .Setup(x => x.GetWithResponseCode<GetFeedbackTransactionResponse>(It.IsAny<GetFeedbackTransactionRequest>()))
                .ReturnsAsync(new ApiResponse<GetFeedbackTransactionResponse>(feedbackTransaction, HttpStatusCode.OK, string.Empty));

            var exception = Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.That(exception.Message, Does.Contain($"No matching template found for templateName: {feedbackTransaction.TemplateName}"));
        }

        [Test]
        public async Task Then_UpdatesTransactionWithZeroCount_WhenNoEligibleUsers()
        {
            var feedbackTransactionId = 12345L;
            var templateId = Guid.NewGuid();

            var request = new TriggerFeedbackEmailsRequest
            {
                NotificationTemplates = new List<NotificationTemplateRequest>
                {
                    new NotificationTemplateRequest
                    {
                        TemplateName = "TestTemplate",
                        TemplateId = templateId
                    }
                },
                EmployerAccountsBaseUrl = "https://employer-accounts.test"
            };

            var feedbackTransaction = new GetFeedbackTransactionResponse
            {
                Id = feedbackTransactionId,
                AccountId = 999,
                AccountName = "Test Company",
                TemplateName = "TestTemplate",
                SendAfter = DateTime.UtcNow.AddDays(-1),
                SentDate = null
            };

            var command = new TriggerFeedbackEmailsCommand(feedbackTransactionId, request);

            _mockEmployerFeedbackApiClient
                .Setup(x => x.GetWithResponseCode<GetFeedbackTransactionResponse>(It.IsAny<GetFeedbackTransactionRequest>()))
                .ReturnsAsync(new ApiResponse<GetFeedbackTransactionResponse>(feedbackTransaction, HttpStatusCode.OK, string.Empty));

            _mockAccountsApiClient
                .Setup(x => x.GetWithResponseCode<GetAccountUsersResponse>(It.IsAny<GetAccountUsersRequest>()))
                .ReturnsAsync(new ApiResponse<GetAccountUsersResponse>(new GetAccountUsersResponse(), HttpStatusCode.OK, string.Empty));

            await _handler.Handle(command, CancellationToken.None);

            _mockEmployerFeedbackApiClient.Verify(x => x.Put<UpdateFeedbackTransactionData>(
                It.Is<UpdateFeedbackTransactionRequest>(r =>
                    r.Data.TemplateId == templateId &&
                    r.Data.SentCount == 0)), Times.Once);

            _mockNotificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Never);
        }
    }
}