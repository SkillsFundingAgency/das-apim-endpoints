using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Finance;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses.Finance;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetPledges
{
    public class GetPledgesQueryHandler : IRequestHandler<GetPledgesQuery, GetPledgesQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IFinanceApiClient<FinanceApiConfiguration> _financeApiClient;


        public GetPledgesQueryHandler(ILevyTransferMatchingService levyTransferMatchingService, IFinanceApiClient<FinanceApiConfiguration> financeApiClient)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _financeApiClient = financeApiClient;
        }

        public async Task<GetPledgesQueryResult> Handle(GetPledgesQuery request, CancellationToken cancellationToken)
        {
            var ltmTask = _levyTransferMatchingService.GetPledges(new GetPledgesRequest(request.AccountId));

            var fundingTask = _financeApiClient.Get<GetTransferAllowanceResponse>(new GetTransferAllowanceByAccountIdRequest(request.AccountId));

            var approvedTask = _levyTransferMatchingService.GetApplications(new GetApplicationsRequest
            {
                SenderAccountId = request.AccountId,
                ApplicationStatusFilter = ApplicationStatus.Approved
            });
            var acceptedTask = _levyTransferMatchingService.GetApplications(new GetApplicationsRequest
            {
                SenderAccountId = request.AccountId,
                ApplicationStatusFilter = ApplicationStatus.Accepted
            });

            await Task.WhenAll(ltmTask, fundingTask, approvedTask, acceptedTask);

            var ltmResponse = await ltmTask;
            var fundingResponse = await fundingTask;
            var approvedResponse = await approvedTask;
            var acceptedResponse = await acceptedTask;

            var applicationsResponse = approvedResponse.Applications.Concat(acceptedResponse.Applications);

            return new GetPledgesQueryResult
            {
                StartingTransferAllowance = fundingResponse.StartingTransferAllowance ?? 0,
                AcceptedAndApprovedApplications = applicationsResponse?.Select(x => (PledgeApplication)x),
                Pledges = ltmResponse.Pledges.Select(x => new GetPledgesQueryResult.Pledge
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    RemainingAmount = x.RemainingAmount,
                    ApplicationCount = x.ApplicationCount,
                    Status = x.Status
                })
            };
        }
    }
}
