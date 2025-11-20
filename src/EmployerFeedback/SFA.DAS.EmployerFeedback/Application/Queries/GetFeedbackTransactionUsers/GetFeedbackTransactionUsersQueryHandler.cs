using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;
using SFA.DAS.EmployerFeedback.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetFeedbackTransactionUsers
{
    public class GetFeedbackTransactionUsersQueryHandler : IRequestHandler<GetFeedbackTransactionUsersQuery, GetFeedbackTransactionUsersResult>
    {
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _employerFeedbackApiClient;
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;
        private readonly ILogger<GetFeedbackTransactionUsersQueryHandler> _logger;

        public GetFeedbackTransactionUsersQueryHandler(
            IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> employerFeedbackApiClient,
            IAccountsApiClient<AccountsConfiguration> accountsApiClient,
            ILogger<GetFeedbackTransactionUsersQueryHandler> logger)
        {
            _employerFeedbackApiClient = employerFeedbackApiClient;
            _accountsApiClient = accountsApiClient;
            _logger = logger;
        }

        public async Task<GetFeedbackTransactionUsersResult> Handle(GetFeedbackTransactionUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing GetFeedbackTransactionUsers for ID: {FeedbackTransactionId}", request.FeedbackTransactionId);

            var feedbackTransactionResponse = await _employerFeedbackApiClient.GetWithResponseCode<GetFeedbackTransactionResponse>(
            new GetFeedbackTransactionRequest(request.FeedbackTransactionId));

            feedbackTransactionResponse.EnsureSuccessStatusCode();

            var feedbackTransaction = feedbackTransactionResponse.Body;
            if (feedbackTransaction == null)
            {
                throw new InvalidOperationException($"Feedback transaction {request.FeedbackTransactionId} not found");
            }

            if (feedbackTransaction.SendAfter > DateTime.UtcNow)
            {
                throw new InvalidOperationException("Feedback transaction sendAfter date is in the future");
            }

            if (feedbackTransaction.SentDate.HasValue)
            {
                _logger.LogInformation("Feedback transaction {FeedbackTransactionId} already sent on {SentDate}",
                       request.FeedbackTransactionId, feedbackTransaction.SentDate);
                return null;
            }

            var accountUsers = await GetEligibleAccountUsers(feedbackTransaction.AccountId);

            var users = accountUsers?.Select(u => new FeedbackTransactionUser
            {
                Name = u.FirstName,
                Email = u.Email
            }).ToList() ?? new List<FeedbackTransactionUser>();

            return new GetFeedbackTransactionUsersResult
            {
                AccountId = feedbackTransaction.AccountId,
                AccountName = feedbackTransaction.AccountName,
                TemplateName = feedbackTransaction.TemplateName,
                Users = users
            };
        }

        private async Task<List<GetAccountTeamMembersResponse>> GetEligibleAccountUsers(long accountId)
        {
            var accountUsersResponse =
                await _accountsApiClient.GetAll<GetAccountTeamMembersResponse>(
                    new GetAccountTeamMembersRequest(accountId));

            return accountUsersResponse?
                .Where(u => u.CanReceiveNotifications && u.Status == 2)
                .ToList();
        }
    }
}