using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFinance;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFinance;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetTasks
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, GetTasksQueryResult>
    {
        private readonly ILogger<GetTasksQueryHandler> _logger;
        private readonly ICurrentDateTime _currentDateTime;
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApi;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _ltmApiClient;
        private readonly IFinanceApiClient<FinanceApiConfiguration> _financeApiClient;

        public GetTasksQueryHandler(
            ILogger<GetTasksQueryHandler> logger, 
            ICurrentDateTime currentDateTime, 
            ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> ltmApiClient, 
            ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient, 
            IAccountsApiClient<AccountsConfiguration> accountsApi,
            IFinanceApiClient<FinanceApiConfiguration> financeApiClient)
        {
            _logger = logger;
            _financeApiClient = financeApiClient;
            _currentDateTime = currentDateTime;
            _accountsApi = accountsApi;
            _ltmApiClient = ltmApiClient;
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
        }

        public async Task<GetTasksQueryResult> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Getting Tasks for account {request.AccountId}");

            var accountTask = _accountsApi.Get<GetAccountByIdResponse>(new GetAccountByIdRequest(request.AccountId));
            var pledgeApplicationsToReviewTask = _ltmApiClient.Get<GetApplicationsResponse>(new GetApplicationsRequest
            {
                SenderAccountId = request.AccountId,
                ApplicationStatusFilter = ApplicationStatus.Pending
            });
            
            var apprenticeChangesTask = _commitmentsV2ApiClient.Get<GetApprenticeshipUpdatesResponse>(new GetPendingApprenticeChangesRequest(request.AccountId));

            var transferRequestsTask = _commitmentsV2ApiClient.Get<GetTransferRequestSummaryResponse>(new GetTransferRequestsRequest(request.AccountId, TransferType.AsSender));
            
            var pendingTransferConnectionsTask = _financeApiClient.Get<List<GetTransferConnectionsResponse.TransferConnection>>(
             new GetTransferConnectionsRequest
             {
                 AccountId = request.AccountId,
                 Status = TransferConnectionInvitationStatus.Pending
             });
            var cohortsToReviewTask = _commitmentsV2ApiClient.Get<GetCohortsResponse>(new GetCohortsRequest { AccountId = request.AccountId });

            await Task.WhenAll(accountTask, pledgeApplicationsToReviewTask, apprenticeChangesTask, transferRequestsTask, pendingTransferConnectionsTask, cohortsToReviewTask);

            var cohortsForThisAccount = await cohortsToReviewTask;
            var cohortsToReview = cohortsForThisAccount?.Cohorts?.Where(x => !x.IsDraft && x.WithParty == Party.Employer);
            var apprenticeChanges = await apprenticeChangesTask;
            var apprenticeChangesCount = apprenticeChanges?.ApprenticeshipUpdates?.Count ?? 0;
            var pendingTransferConnections = await pendingTransferConnectionsTask;

            var pledgeApplicationsToReview = await pledgeApplicationsToReviewTask;
            var account = await accountTask;
            var transferRequests = await transferRequestsTask;

            var pendingTransferRequestsRequestsToReview = transferRequests?.TransferRequestSummaryResponse?.Where(x => x.Status == TransferApprovalStatus.Pending);

            return new GetTasksQueryResult()
            {
                NumberOfCohortsReadyToReview = cohortsToReview?.Count() ?? 0,
                NumberTransferPledgeApplicationsToReview = pledgeApplicationsToReview?.TotalItems ?? 0,
                NumberOfPendingTransferConnections = pendingTransferConnections?.Count() ?? 0,
                ShowLevyDeclarationTask = account?.ApprenticeshipEmployerType == ApprenticeshipEmployerType.Levy && IsInDateRange(),
                NumberOfTransferRequestToReview = pendingTransferRequestsRequestsToReview?.Count() ?? 0,
                NumberOfApprenticesToReview = apprenticeChangesCount
            };
        }

        private bool IsInDateRange()
        {
            int dayOfMonth = _currentDateTime.Now.Day;
            var isInDateRange = dayOfMonth >= 16 && dayOfMonth < 20;
            return isInDateRange;
        }
    }
}