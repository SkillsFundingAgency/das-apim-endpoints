using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
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
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _ltmApiClient;

        public GetTasksQueryHandler(ILogger<GetTasksQueryHandler> logger, ICurrentDateTime currentDateTime, IAccountsApiClient<AccountsConfiguration> accountsApi, ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> ltmApiClient)
        {
            _logger = logger;
            _currentDateTime = currentDateTime;
            _accountsApi = accountsApi;
            _ltmApiClient = ltmApiClient;
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

            await Task.WhenAll(pledgeApplicationsToReviewTask, accountTask);

            var pledgeApplicationsToReview = await pledgeApplicationsToReviewTask;
            var account = await accountTask;

            return new GetTasksQueryResult()
            {
                ShowLevyDeclarationTask = account?.ApprenticeshipEmployerType == ApprenticeshipEmployerType.Levy && isInDateRange,
                NumberTransferPledgeApplicationsToReview = pledgeApplicationsToReview?.TotalItems ?? 0
            };
        }
    }
}