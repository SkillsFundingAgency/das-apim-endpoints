using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFinance;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFinance;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetTasks
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, GetTasksQueryResult>
    {
        private readonly ILogger<GetTasksQueryHandler> _logger;
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _ltmApiClient;
        private readonly IFinanceApiClient<FinanceApiConfiguration> _financeApiClient;

        public GetTasksQueryHandler(ILogger<GetTasksQueryHandler> logger, ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> ltmApiClient, IFinanceApiClient<FinanceApiConfiguration> financeApiClient)
        {
            _logger = logger;
            _financeApiClient = financeApiClient;
            _ltmApiClient = ltmApiClient;
        }

        public async Task<GetTasksQueryResult> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Getting Tasks for account {request.AccountId}");

            var pledgeApplicationsToReviewTask = _ltmApiClient.Get<GetApplicationsResponse>(new GetApplicationsRequest
            {
                SenderAccountId = request.AccountId,
                ApplicationStatusFilter = ApplicationStatus.Pending
            });

            var pendingTransferConnectionsTask = _financeApiClient.Get<List<GetTransferConnectionsResponse.TransferConnection>>(
             new GetTransferConnectionsRequest
             {
                 AccountId = request.AccountId,
                 Status = TransferConnectionInvitationStatus.Pending
             });

            await Task.WhenAll(pledgeApplicationsToReviewTask, pendingTransferConnectionsTask);
            var pledgeApplicationsToReview = await pledgeApplicationsToReviewTask;

            var pendingTransferConnections = await pendingTransferConnectionsTask;

            return new GetTasksQueryResult()
            {
                NumberTransferPledgeApplicationsToReview = pledgeApplicationsToReview?.TotalItems ?? 0,
                NumberOfPendingTransferConnections = pendingTransferConnections?.Count() ?? 0
            };
        }
    }
}