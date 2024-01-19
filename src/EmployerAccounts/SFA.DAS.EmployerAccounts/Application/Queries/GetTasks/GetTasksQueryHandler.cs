using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetTasks
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, GetTasksQueryResult>
    {
        private readonly ILogger<GetTasksQueryHandler> _logger;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;

        public GetTasksQueryHandler(ILogger<GetTasksQueryHandler> logger, ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        {
            _logger = logger;
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
        }

        public async Task<GetTasksQueryResult> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Getting Tasks for account {request.AccountId}");

            var apprenticeChangesTask = _commitmentsV2ApiClient.Get<GetApprenticeshipUpdatesResponse>(new GetPendingApprenticeChangesRequest(request.AccountId));

            await Task.WhenAll(apprenticeChangesTask);
            var apprenticeChanges = await apprenticeChangesTask;

            var apprenticeChangesCount = apprenticeChanges?.ApprenticeshipUpdates?.Count ?? 0;

            return new GetTasksQueryResult()
            {
                NumberOfApprenticesToReview = apprenticeChangesCount
            };
        }
    }
}