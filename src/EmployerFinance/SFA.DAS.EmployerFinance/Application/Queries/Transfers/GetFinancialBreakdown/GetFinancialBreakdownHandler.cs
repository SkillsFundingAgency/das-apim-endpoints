using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerFinance.Models.Constants;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetFinancialBreakdown
{
    public class GetFinancialBreakdownHandler(
        ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> levyTransferMatchingApiClient)
        : IRequestHandler<GetFinancialBreakdownQuery, GetFinancialBreakdownResult>
    {
        public async Task<GetFinancialBreakdownResult> Handle(GetFinancialBreakdownQuery request, CancellationToken cancellationToken)
        {
            var pledgesTask = levyTransferMatchingApiClient.Get<GetPledgesResponse>
                                                        (new GetPledgesRequest(request.AccountId));
            await Task.WhenAll(pledgesTask);
            return new GetFinancialBreakdownResult
            {
                Commitments = 0,
                ApprovedPledgeApplications = 0,
                TransferConnections = 0,
                AcceptedPledgeApplications = 0,
                PledgeOriginatedCommitments = 0,                
                AmountPledged = pledgesTask.Result.Pledges.Where(p => p.Status != PledgeStatus.Closed).Sum(x => x.Amount)
            };
        }

    }
}
