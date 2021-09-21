using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using GetApplicationsRequest = SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests.Applications.GetApplicationsRequest;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplications
{
    public class GetApplicationsQueryHandler : IRequestHandler<GetApplicationsQuery, GetApplicationsResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IAccountsService _accountsService;

        public GetApplicationsQueryHandler(ILevyTransferMatchingService levyTransferMatchingService, IAccountsService accountsService)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _accountsService = accountsService;
        }

        public async Task<GetApplicationsResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            var ltm = _levyTransferMatchingService.GetApplications(new GetApplicationsRequest
            {
                AccountId = request.AccountId
            });

            Task.WaitAll(new Task[] {ltm}, cancellationToken);

            return new GetApplicationsResult
            {
                Applications = ltm.Result.Applications.ToList()
            };
        }
    }
}
