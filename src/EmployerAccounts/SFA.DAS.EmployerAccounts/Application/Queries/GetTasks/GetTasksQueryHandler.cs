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

            var transferRequestsTask = _commitmentsV2ApiClient.Get<GetTransferRequestSummaryResponse>(new GetTransferRequestsRequest(request.AccountId));

            await Task.WhenAll(pledgeApplicationsToReviewTask, transferRequestsTask);

            var pledgeApplicationsToReview = await pledgeApplicationsToReviewTask;

            var transferRequests = await transferRequestsTask;

            var pendingTransfertransferRequestsRequestsToReview = transferRequests?.TransferRequestSummaryResponse?.Where(x => x.Status == TransferApprovalStatus.Pending);

            return new GetTasksQueryResult()
            {
                NumberTransferPledgeApplicationsToReview = pledgeApplicationsToReview?.TotalItems ?? 0,
                NumberOfTransferRequestToReview = pendingTransfertransferRequestsRequestsToReview?.Count() ?? 0
            };
        }
    }
}