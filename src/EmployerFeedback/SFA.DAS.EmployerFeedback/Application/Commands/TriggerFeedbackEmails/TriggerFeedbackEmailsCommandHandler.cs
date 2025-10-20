using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;
using SFA.DAS.EmployerFeedback.Models;
using SFA.DAS.Encoding;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Application.Commands.TriggerFeedbackEmails
{
    public class TriggerFeedbackEmailsCommandHandler : IRequestHandler<TriggerFeedbackEmailsCommand>
    {
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _employerFeedbackApiClient;
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;
        private readonly INotificationService _notificationService;
        private readonly ILogger<TriggerFeedbackEmailsCommandHandler> _logger;
        private readonly IEncodingService _encodingService;

        public TriggerFeedbackEmailsCommandHandler(
            IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> employerFeedbackApiClient,
            IAccountsApiClient<AccountsConfiguration> accountsApiClient,
            INotificationService notificationService,
            ILogger<TriggerFeedbackEmailsCommandHandler> logger,
            IEncodingService encodingService)
        {
            _employerFeedbackApiClient = employerFeedbackApiClient;
            _accountsApiClient = accountsApiClient;
            _notificationService = notificationService;
            _encodingService = encodingService;
            _logger = logger;
        }

        public async Task Handle(TriggerFeedbackEmailsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing TriggerFeedbackEmails for ID: {FeedbackTransactionId}", request.FeedbackTransactionId);

            var feedbackTransaction = await GetAndValidateFeedbackTransaction(request.FeedbackTransactionId);

            ValidateBusinessRules(feedbackTransaction, request.FeedbackTransactionId);

            if (feedbackTransaction.SentDate.HasValue)
            {
                _logger.LogInformation("Feedback transaction {FeedbackTransactionId} already sent on {SentDate}",
                    request.FeedbackTransactionId, feedbackTransaction.SentDate);
                return;
            }

            var matchingTemplate = FindMatchingTemplate(feedbackTransaction.TemplateName, request.Request.NotificationTemplates);

            var accountUsers = await GetEligibleAccountUsers(feedbackTransaction.AccountId);

            if (accountUsers == null || accountUsers.Count == 0)
            {
                _logger.LogInformation("No eligible users found for account {AccountId}", feedbackTransaction.AccountId);
                await UpdateFeedbackTransaction(request.FeedbackTransactionId, matchingTemplate.TemplateId, 0);
                return;
            }

            await UpdateFeedbackTransaction(request.FeedbackTransactionId, matchingTemplate.TemplateId, accountUsers.Count);

            await SendEmailsToUsers(accountUsers, feedbackTransaction, matchingTemplate, request.Request.EmployerAccountsBaseUrl);

            _logger.LogInformation("Successfully sent {Count} email notifications for feedback transaction {FeedbackTransactionId}",
                accountUsers.Count, request.FeedbackTransactionId);
        }

        private async Task<GetFeedbackTransactionResponse> GetAndValidateFeedbackTransaction(long feedbackTransactionId)
        {
            var feedbackTransactionResponse = await _employerFeedbackApiClient.GetWithResponseCode<GetFeedbackTransactionResponse>(
                new GetFeedbackTransactionRequest(feedbackTransactionId));

            var feedbackTransaction = feedbackTransactionResponse.Body;
            if (feedbackTransaction == null)
            {
                throw new InvalidOperationException($"Feedback transaction {feedbackTransactionId} not found");
            }

            return feedbackTransaction;
        }

        private void ValidateBusinessRules(GetFeedbackTransactionResponse feedbackTransaction, long feedbackTransactionId)
        {
            if (feedbackTransaction.SendAfter > DateTime.UtcNow)
            {
                throw new InvalidOperationException("Feedback transaction sendAfter date is in the future");
            }
        }

        private NotificationTemplateRequest FindMatchingTemplate(string templateName, List<NotificationTemplateRequest> notificationTemplates)
        {
            var matchingTemplate = notificationTemplates?
                .FirstOrDefault(t => string.Equals(t.TemplateName, templateName, StringComparison.OrdinalIgnoreCase));

            if (matchingTemplate == null)
            {
                _logger.LogInformation($"No matching template found for templateName: {templateName}");
                throw new InvalidOperationException($"No matching template found for templateName: {templateName}");
            }

            return matchingTemplate;
        }

        private async Task<List<AccountUser>> GetEligibleAccountUsers(long accountId)
        {
            var accountUsersResponse = await _accountsApiClient.GetWithResponseCode<GetAccountUsersResponse>(
                new GetAccountUsersRequest(accountId));
            accountUsersResponse.EnsureSuccessStatusCode();

            return accountUsersResponse.Body?
                .Where(u => u.CanReceiveNotifications && u.Status == 2)
                .ToList();
        }

        private async Task SendEmailsToUsers(
            List<AccountUser> accountUsers,
            GetFeedbackTransactionResponse feedbackTransaction,
            NotificationTemplateRequest template,
            string employerAccountsBaseUrl)
        {
            var hashedAccountId = _encodingService.Encode(feedbackTransaction.AccountId, Encoding.EncodingType.AccountId);

            foreach (var user in accountUsers)
            {
                var tokens = CreateEmailTokens(user, feedbackTransaction, employerAccountsBaseUrl, hashedAccountId);

                var emailCommand = new SendEmailCommand(
                    templateId: template.TemplateId.ToString(),
                    recipientsAddress: user.Email,
                    tokens: tokens);

                await _notificationService.Send(emailCommand);

                _logger.LogDebug("Sent email notification for user {Email} on account {AccountId}",
                    user.Email, feedbackTransaction.AccountId);
            }
        }

        private Dictionary<string, string> CreateEmailTokens(
            AccountUser user,
            GetFeedbackTransactionResponse feedbackTransaction,
            string employerAccountsBaseUrl,
            string hashedAccountId)
        {
            return new Dictionary<string, string>
            {
                { "contact", user.FirstName },
                { "employername", feedbackTransaction.AccountName },
                { "employeraccountsweb", employerAccountsBaseUrl },
                { "accounthashedid", hashedAccountId }
            };
        }

        private async Task UpdateFeedbackTransaction(long feedbackTransactionId, Guid templateId, int sentCount)
        {
            var updateData = new UpdateFeedbackTransactionData
            {
                TemplateId = templateId,
                SentCount = sentCount,
                SentDate = DateTime.UtcNow
            };

            await _employerFeedbackApiClient.Put<UpdateFeedbackTransactionData>(
                new UpdateFeedbackTransactionRequest(feedbackTransactionId, updateData));
        }
    }
}