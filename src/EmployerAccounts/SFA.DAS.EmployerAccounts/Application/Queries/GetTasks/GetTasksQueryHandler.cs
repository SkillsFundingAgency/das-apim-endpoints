using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetTasks
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, GetTasksQueryResult>
    {
        private readonly ILogger<GetTasksQueryHandler> _logger;
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _ltmApiClient;

        public GetTasksQueryHandler(ILogger<GetTasksQueryHandler> logger, ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> ltmApiClient)
        {
            _logger = logger;
            _ltmApiClient = ltmApiClient;
        }

        public async Task<GetTasksQueryResult> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Getting Tasks for account {request.AccountId}");

            var pledgeApplicationsToReviewTask = _ltmApiClient.Get<GetNumberTransferPledgeApplicationsToReviewResponse>(
                new GetNumberTransferPledgeApplicationsToReviewRequest(request.AccountId)
            );

            await Task.WhenAll(pledgeApplicationsToReviewTask);
            var pledgeApplicationsToReview = await pledgeApplicationsToReviewTask;

            return new GetTasksQueryResult()
            {
                NumberTransferPledgeApplicationsToReview = pledgeApplicationsToReview?.NumberTransferPledgeApplicationsToReview ?? 0
            };
        }
    }
}