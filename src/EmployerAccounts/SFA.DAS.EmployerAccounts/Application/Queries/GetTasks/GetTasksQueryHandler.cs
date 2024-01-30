using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetTasks
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, GetTasksQueryResult>
    {
        private readonly ILogger<GetTasksQueryHandler> _logger;
        private readonly ICurrentDateTime _currentDateTime;
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApi;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _ltmApiClient;

        public GetTasksQueryHandler(ILogger<GetTasksQueryHandler> logger, ICurrentDateTime currentDateTime, ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> ltmApiClient, ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient, IAccountsApiClient<AccountsConfiguration> accountsApi)
        {
            _logger = logger;
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

            int dayOfMonth = _currentDateTime.Now.Day;
            var isInDateRange = dayOfMonth >= 16 && dayOfMonth < 20;

            var apprenticeChangesTask = _commitmentsV2ApiClient.Get<GetApprenticeshipUpdatesResponse>(new GetPendingApprenticeChangesRequest(request.AccountId));

            await Task.WhenAll(apprenticeChangesTask, pledgeApplicationsToReviewTask, accountTask);

            var apprenticeChanges = await apprenticeChangesTask;
            var apprenticeChangesCount = apprenticeChanges?.ApprenticeshipUpdates?.Count ?? 0;

            var pledgeApplicationsToReview = await pledgeApplicationsToReviewTask;
            var account = await accountTask;

            return new GetTasksQueryResult()
            {
                NumberTransferPledgeApplicationsToReview = pledgeApplicationsToReview?.TotalItems ?? 0,
                NumberOfApprenticesToReview = apprenticeChangesCount,
                ShowLevyDeclarationTask = account?.ApprenticeshipEmployerType == ApprenticeshipEmployerType.Levy && isInDateRange
            };
        }
    }
}