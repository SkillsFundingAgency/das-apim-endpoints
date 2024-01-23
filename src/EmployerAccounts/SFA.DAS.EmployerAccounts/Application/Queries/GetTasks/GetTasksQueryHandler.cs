using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetTasks
{
    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, GetTasksQueryResult>
    {
        private readonly ILogger<GetTasksQueryHandler> _logger;
        private readonly ICurrentDateTime _currentDateTime;
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApi;

        public GetTasksQueryHandler(ILogger<GetTasksQueryHandler> logger, ICurrentDateTime currentDateTime, IAccountsApiClient<AccountsConfiguration> accountsApi)
        {
            _logger = logger;
            _currentDateTime = currentDateTime;
            _accountsApi = accountsApi;
        }

        public async Task<GetTasksQueryResult> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Getting Tasks for account {request.AccountId}");
            var accountTask = _accountsApi.Get<GetAccountByIdResponse>(new GetAccountByIdRequest(request.AccountId));

            await Task.WhenAll(accountTask);
            var account = await accountTask;

            int dayOfMonth = _currentDateTime.Now.Day;
            var isInDateRange = dayOfMonth >= 16 && dayOfMonth < 20;

            return new GetTasksQueryResult()
            {
                ShowLevyDeclarationTask = account?.ApprenticeshipEmployerType == ApprenticeshipEmployerType.Levy && isInDateRange
            };
        }
    }
}