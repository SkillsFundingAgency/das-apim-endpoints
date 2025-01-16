using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetTasks;

public class GetTasksQueryHandler(
    ILogger<GetTasksQueryHandler> logger,
    ICurrentDateTime currentDateTime,
    ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> ltmApiClient,
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient,
    IAccountsApiClient<AccountsConfiguration> accountsApi,
    IFinanceApiClient<FinanceApiConfiguration> financeApiClient)
    : IRequestHandler<GetTasksQuery, GetTasksQueryResult>
{
    public async Task<GetTasksQueryResult> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Tasks for account {AccountId}", request.AccountId);

        var accountTask = accountsApi.Get<GetAccountByIdResponse>(new GetAccountByIdRequest(request.AccountId));
        var pledgeApplicationsToReviewTask = ltmApiClient.Get<GetApplicationsResponse>(new GetApplicationsRequest
        {
            SenderAccountId = request.AccountId,
            ApplicationStatusFilter = ApplicationStatus.Pending
        });

        var approvedPledgeApplicationsTask = ltmApiClient.Get<GetApplicationsResponse>(new GetApplicationsRequest
        {
            SenderAccountId = request.AccountId,
            ApplicationStatusFilter = ApplicationStatus.Approved
        });

        var apprenticeChangesTask = commitmentsV2ApiClient.Get<GetApprenticeshipUpdatesResponse>(new GetPendingApprenticeChangesRequest(request.AccountId));

        var transferRequestsTask = commitmentsV2ApiClient.Get<GetTransferRequestSummaryResponse>(new GetTransferRequestsRequest(request.AccountId, TransferType.AsSender));

        var pendingTransferConnectionsTask = financeApiClient.Get<List<GetTransferConnectionsResponse.TransferConnection>>(
            new GetTransferConnectionsRequest
            {
                AccountId = request.AccountId,
                Status = TransferConnectionInvitationStatus.Pending
            });

        var cohortsToReviewTask = commitmentsV2ApiClient.Get<GetCohortsResponse>(new GetCohortsRequest { AccountId = request.AccountId });

        await Task.WhenAll(
            accountTask,
            pledgeApplicationsToReviewTask,
            apprenticeChangesTask,
            transferRequestsTask,
            pendingTransferConnectionsTask,
            cohortsToReviewTask,
            approvedPledgeApplicationsTask
            );

        var cohortsForThisAccount = await cohortsToReviewTask;
        var cohortsToReview = cohortsForThisAccount?.Cohorts?.Where(x => !x.IsDraft && x.WithParty == Party.Employer);
        var apprenticeChanges = await apprenticeChangesTask;
        var apprenticeChangesCount = apprenticeChanges?.ApprenticeshipUpdates?.Count ?? 0;
        var pendingTransferConnections = await pendingTransferConnectionsTask;
        var approvedPledgeApplications = await approvedPledgeApplicationsTask;
        
        logger.LogInformation("GetTasksQueryHandler approvedPledgeApplications: {Data}", JsonConvert.SerializeObject(approvedPledgeApplications));

        var pledgeApplicationsToReview = await pledgeApplicationsToReviewTask;
        var account = await accountTask;
        var transferRequests = await transferRequestsTask;

        var pendingTransferRequestsRequestsToReviewCount = transferRequests?.TransferRequestSummaryResponse?.Count(x => x.Status == TransferApprovalStatus.Pending);
        
        return new GetTasksQueryResult
        {
            NumberOfCohortsReadyToReview = cohortsToReview?.Count() ?? 0,
            NumberTransferPledgeApplicationsToReview = pledgeApplicationsToReview?.TotalItems ?? 0,
            NumberOfPendingTransferConnections = pendingTransferConnections?.Count ?? 0,
            ShowLevyDeclarationTask = account?.ApprenticeshipEmployerType == ApprenticeshipEmployerType.Levy && IsInDateRange(),
            NumberOfTransferRequestToReview = pendingTransferRequestsRequestsToReviewCount ?? 0,
            NumberOfTransferPledgeApplicationsApproved = approvedPledgeApplications?.TotalItems ?? 0,
            NumberOfApprenticesToReview = apprenticeChangesCount,
            SingleApprovedTransferPledgeId = approvedPledgeApplications?.TotalItems == 1 ? approvedPledgeApplications.Applications.First().PledgeId : null
        };
    }

    private bool IsInDateRange()
    {
        int dayOfMonth = currentDateTime.Now.Day;
        var isInDateRange = dayOfMonth >= 16 && dayOfMonth < 20;
        return isInDateRange;
    }
}