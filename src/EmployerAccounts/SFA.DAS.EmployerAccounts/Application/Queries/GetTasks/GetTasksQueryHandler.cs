using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetTasks
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, GetTasksQueryResult>
    {
        private readonly ILogger<GetTasksQueryHandler> _logger;

        public GetTasksQueryHandler(ILogger<GetTasksQueryHandler> logger)
        {
            _logger = logger;
        }

        public async Task<GetTasksQueryResult> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Getting Tasks for account {request.AccountId}");

            return new GetTasksQueryResult()
            {
            };
        }
    }
}