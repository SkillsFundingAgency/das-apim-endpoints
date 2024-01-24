using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using System.Linq;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetTasks
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, GetTasksQueryResult>
    {
        private readonly ILogger<GetTasksQueryHandler> _logger;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;

        public GetTasksQueryHandler(ILogger<GetTasksQueryHandler> logger, ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        {
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
            _logger = logger;
        }

        public async Task<GetTasksQueryResult> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Getting Tasks for account {request.AccountId}");

            var transferRequestsTask = _commitmentsV2ApiClient.Get<GetTransferRequestSummaryResponse>(new GetTransferRequestsRequest(request.AccountId));

            await Task.WhenAll(transferRequestsTask);
            var transferRequests = await transferRequestsTask;

            var pendingTransfertransferRequestsRequestsToReview = transferRequests?.TransferRequestSummaryResponse?.Where(x => x.Status == TransferApprovalStatus.Pending);

            return new GetTasksQueryResult()
            {
                NumberOfTransferRequestToReview = pendingTransfertransferRequestsRequestsToReview?.Count() ?? 0
            };
        }
    }
}