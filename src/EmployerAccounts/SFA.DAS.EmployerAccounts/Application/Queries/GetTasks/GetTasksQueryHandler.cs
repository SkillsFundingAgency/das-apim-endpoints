using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetTasks
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, GetTasksQueryResult>
    {
        private readonly ILogger<GetTasksQueryHandler> _logger;
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _ltmApiClient;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;

        public GetTasksQueryHandler(ILogger<GetTasksQueryHandler> logger, ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> ltmApiClient, ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        {
            _logger = logger;
            _ltmApiClient = ltmApiClient;
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
        }

        public async Task<GetTasksQueryResult> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Getting Tasks for account {request.AccountId}");

            var pledgeApplicationsToReviewTask = _ltmApiClient.Get<GetApplicationsResponse>(new GetApplicationsRequest
            {
                SenderAccountId = request.AccountId,
                ApplicationStatusFilter = ApplicationStatus.Pending
            });

            var cohortsToReviewTask = _commitmentsV2ApiClient.Get<GetCohortsResponse>(new GetCohortsRequest { AccountId = request.AccountId });

            await Task.WhenAll(pledgeApplicationsToReviewTask, cohortsToReviewTask);

            var pledgeApplicationsToReview = await pledgeApplicationsToReviewTask;

            var cohortsForThisAccount = await cohortsToReviewTask;
            var cohortsToReview = cohortsForThisAccount.Cohorts?.Where(x => !x.IsDraft && x.WithParty == Party.Employer);

            return new GetTasksQueryResult()
            {
                NumberOfCohortsReadyToReview = cohortsToReview?.Count() ?? 0,
                NumberTransferPledgeApplicationsToReview = pledgeApplicationsToReview?.TotalItems ?? 0
            };
        }
    }
}