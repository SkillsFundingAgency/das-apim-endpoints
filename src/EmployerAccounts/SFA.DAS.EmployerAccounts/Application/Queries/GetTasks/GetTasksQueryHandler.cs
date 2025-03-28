using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            ApplicationStatusFilter = ApplicationStatus.Pending,
        });
        
        var approvedPledgeApplicationsTask = ltmApiClient.Get<GetApplicationsResponse>(new GetApplicationsRequest
        {
            AccountId = request.AccountId,
            ApplicationStatusFilter = ApplicationStatus.Approved
        });

        var acceptedPledgeApplicationsTask = ltmApiClient.Get<GetApplicationsResponse>(new GetApplicationsRequest
        {
            AccountId = request.AccountId,
            ApplicationStatusFilter = ApplicationStatus.Accepted
        });
        
        var apprenticeChangesTask = commitmentsV2ApiClient.Get<GetApprenticeshipUpdatesResponse>(new GetPendingApprenticeChangesRequest(request.AccountId));

        var transferRequestsTask = commitmentsV2ApiClient.Get<GetTransferRequestSummaryResponse>(new GetTransferRequestsRequest(request.AccountId, TransferType.AsSender));

        var pendingTransferConnectionsTask = financeApiClient.Get<List<GetTransferConnectionsResponse.TransferConnection>>(new GetTransferConnectionsRequest
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
            acceptedPledgeApplicationsTask,
            approvedPledgeApplicationsTask
        );

        var cohortsForThisAccountResponse = await cohortsToReviewTask;
        var cohortsForThisAccount = cohortsForThisAccountResponse.Cohorts?.ToList();
        var cohortsToReview = cohortsForThisAccountResponse.Cohorts?.Where(x => !x.IsDraft && x.WithParty == Party.Employer).ToList();
        var apprenticeChanges = await apprenticeChangesTask;
        var apprenticeChangesCount = apprenticeChanges?.ApprenticeshipUpdates?.Count ?? 0;
        var pendingTransferConnections = await pendingTransferConnectionsTask;
        var pledgeApplicationsAcceptedResponse = await acceptedPledgeApplicationsTask;
        var pledgeApplicationsToReviewResponse = await pledgeApplicationsToReviewTask;
        var account = await accountTask;
        var transferRequests = await transferRequestsTask;
        var approvedPledgeApplications = await approvedPledgeApplicationsTask;

        var pledgeApplicationsAcceptedIdsWithoutApprentices = GetAcceptedLevyTransfersWithoutApprenticeships(
            [pledgeApplicationsAcceptedResponse],
            cohortsForThisAccount
        );

        var pendingTransferRequestsRequestsToReviewCount = transferRequests?.TransferRequestSummaryResponse?.Count(x => x.Status == TransferApprovalStatus.Pending);
        var isWithinLevyNotificationDateRange = IsWithinLevyNotificationDateRange();
        var showLevyDeclarationTask = account?.ApprenticeshipEmployerType == ApprenticeshipEmployerType.Levy && isWithinLevyNotificationDateRange;
        
        return new GetTasksQueryResult
        {
            NumberOfCohortsReadyToReview = cohortsToReview?.Count ?? 0,
            NumberTransferPledgeApplicationsToReview = pledgeApplicationsToReviewResponse?.TotalItems ?? 0,
            NumberOfPendingTransferConnections = pendingTransferConnections?.Count ?? 0,
            ShowLevyDeclarationTask = showLevyDeclarationTask,
            NumberOfTransferRequestToReview = pendingTransferRequestsRequestsToReviewCount ?? 0,
            NumberOfApprenticesToReview = apprenticeChangesCount,
            NumberOfAcceptedTransferPledgeApplicationsWithNoApprentices = pledgeApplicationsAcceptedIdsWithoutApprentices.Count,
            SingleAcceptedTransferPledgeApplicationIdWithNoApprentices = pledgeApplicationsAcceptedIdsWithoutApprentices.Count == 1 ? pledgeApplicationsAcceptedIdsWithoutApprentices[0] : null,
            NumberOfTransferPledgeApplicationsApproved = approvedPledgeApplications?.TotalItems ?? 0,
            SingleApprovedTransferApplicationId = approvedPledgeApplications?.TotalItems == 1 ? approvedPledgeApplications.Applications.First().Id : null
        };
    }

    private static List<int> GetAcceptedLevyTransfersWithoutApprenticeships(GetApplicationsResponse[] responses, List<GetCohortsResponse.CohortSummary> cohortsForThisAccount)
    {
        var applications = responses
            .Where(x=> x.Applications is not null)
            .SelectMany(x => x.Applications).ToList();
        
        if (applications.Count == 0)
        {
            return [];
        }

        var acceptedApplicationIdsWithoutApprentices = new List<int>();

        foreach (var acceptedApplicationId in applications.Select(x => x.Id))
        {
            if (cohortsForThisAccount == null || cohortsForThisAccount.Count == 0)
            {
                acceptedApplicationIdsWithoutApprentices.Add(acceptedApplicationId);
                continue;
            }

            var cohortsForApplication = cohortsForThisAccount.Where(x => x.PledgeApplicationId == acceptedApplicationId).ToList();

            if (cohortsForApplication.Count == 0 || cohortsForApplication.Any(x => x.NumberOfDraftApprentices == 0))
            {
                acceptedApplicationIdsWithoutApprentices.Add(acceptedApplicationId);
            }
        }

        return acceptedApplicationIdsWithoutApprentices;
    }

    private bool IsWithinLevyNotificationDateRange()
    {
        return currentDateTime.Now.Day is >= 16 and < 20;
    }
}