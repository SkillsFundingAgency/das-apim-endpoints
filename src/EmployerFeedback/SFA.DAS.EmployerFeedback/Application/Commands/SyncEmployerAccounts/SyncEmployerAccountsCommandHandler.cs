using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFeedback.Application.Commands.SyncEmployerAccounts
{
    public class SyncEmployerAccountsCommandHandler : IRequestHandler<SyncEmployerAccountsCommand>
    {
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _feedbackApiClient;
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;
        private readonly ILogger<SyncEmployerAccountsCommandHandler> _logger;
        private const int MaxRetryAttempts = 3;
        private const int PageSize = 1000;

        public SyncEmployerAccountsCommandHandler(
            IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> feedbackApiClient,
            IAccountsApiClient<AccountsConfiguration> accountsApiClient,
            ILogger<SyncEmployerAccountsCommandHandler> logger)
        {
            _feedbackApiClient = feedbackApiClient;
            _accountsApiClient = accountsApiClient;
            _logger = logger;
        }

        public async Task Handle(SyncEmployerAccountsCommand request, CancellationToken cancellationToken)
        {
            DateTime syncStartTime = DateTime.UtcNow;
            DateTime? lastSyncDate = await GetLastSyncDateAsync(cancellationToken);

            int page = 1;
            int totalPages;
            do
            {
                var accountsResponse = await GetAccountsPageAsync(lastSyncDate, page, cancellationToken);
                if (accountsResponse == null)
                {
                    _logger.LogError("SyncEmployerAccounts: Received null response when fetching page {Page}", page);
                    throw new InvalidOperationException($"Failed to retrieve accounts data for page {page}");
                }
                totalPages = accountsResponse.TotalPages;
                var updatedAccounts = accountsResponse.Data;
                if (updatedAccounts == null || updatedAccounts.Count == 0)
                {
                    _logger.LogError("SyncEmployerAccounts: No accounts returned on page {Page}", page);
                    throw new InvalidOperationException($"No accounts returned for page {page}");
                }
                await UpsertAccountsAsync(updatedAccounts, cancellationToken);
                page++;
            } while (page <= totalPages);

            await UpdateSyncSettingsAsync(syncStartTime, cancellationToken);
        }

        private async Task<GetUpdatedEmployerAccountsResponse> GetAccountsPageAsync(DateTime? lastSyncDate, int page, CancellationToken cancellationToken)
        {
            var accountsApiResponse = await ExecuteWithRetry(async () =>
            {
                var response = await _accountsApiClient.GetWithResponseCode<GetUpdatedEmployerAccountsResponse>(
                    new GetUpdatedEmployerAccountsRequest(lastSyncDate, page, PageSize));
                response.EnsureSuccessStatusCode();
                return response;
            }, MaxRetryAttempts, cancellationToken);
            return accountsApiResponse.Body;
        }

        private async Task UpsertAccountsAsync(List<UpdatedEmployerAccounts> updatedAccounts, CancellationToken cancellationToken)
        {
            var upsertData = new List<UpsertAccountsData>();
            foreach (var acc in updatedAccounts)
            {
                upsertData.Add(new UpsertAccountsData
                {
                    AccountId = acc.AccountId,
                    AccountName = acc.AccountName
                });
            }
            await ExecuteWithRetry(async () =>
            {
                var response = await _feedbackApiClient.PostWithResponseCode<AccountsData, object>(
                    new UpsertAccountsRequest(new AccountsData { Accounts = upsertData }), false);
                response.EnsureSuccessStatusCode();
                return response;
            }, MaxRetryAttempts, cancellationToken);
        }

        private async Task UpdateSyncSettingsAsync(DateTime syncStartTime, CancellationToken cancellationToken)
        {
            await ExecuteWithRetry(async () =>
            {
                await _feedbackApiClient.Put(new UpdateSettingsRequest(syncStartTime));
                return true;
            }, MaxRetryAttempts, cancellationToken);
        }

        private async Task<DateTime?> GetLastSyncDateAsync(CancellationToken cancellationToken)
        {
            var settings = await _feedbackApiClient.GetWithResponseCode<GetSettingsResponse>(new GetSettingsRequest());
            settings.EnsureSuccessStatusCode();
            return settings.Body.Value?.AddMinutes(-1);
        }
        private async Task<T> ExecuteWithRetry<T>(Func<Task<T>> func, int maxAttempts, CancellationToken cancellationToken)
        {
            int attempt = 0;
            while (true)
            {
                try
                {
                    return await func();
                }
                catch (Exception ex) when (attempt < maxAttempts - 1)
                {
                    attempt++;
                    _logger.LogWarning(ex, $"Retrying operation (attempt {attempt + 1} of {maxAttempts})");
                    await Task.Delay(TimeSpan.FromSeconds(2 * attempt), cancellationToken);
                }
            }
        }
    }
}
