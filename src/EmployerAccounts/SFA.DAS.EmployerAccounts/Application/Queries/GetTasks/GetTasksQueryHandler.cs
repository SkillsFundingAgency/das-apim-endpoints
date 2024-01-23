using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFinance;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFinance;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetTasks
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, GetTasksQueryResult>
    {
        private readonly ILogger<GetTasksQueryHandler> _logger;
        private readonly IFinanceApiClient<FinanceApiConfiguration> _financeApiClient;

        public GetTasksQueryHandler(ILogger<GetTasksQueryHandler> logger, IFinanceApiClient<FinanceApiConfiguration> financeApiClient)
        {
            _financeApiClient = financeApiClient;
            _logger = logger;
        }

        public async Task<GetTasksQueryResult> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Getting Tasks for account {request.AccountId}");

            var pendingTransferConnectionsTask = _financeApiClient.Get<List<GetTransferConnectionsResponse.TransferConnection>>(
             new GetTransferConnectionsRequest
             {
                 AccountId = request.AccountId,
                 Status = TransferConnectionInvitationStatus.Pending
             });

            await Task.WhenAll(pendingTransferConnectionsTask);
            var pendingTransferConnections = await pendingTransferConnectionsTask;

            return new GetTasksQueryResult()
            {
                NumberOfPendingTransferConnections = pendingTransferConnections?.Count() ?? 0
            };
        }
    }
}