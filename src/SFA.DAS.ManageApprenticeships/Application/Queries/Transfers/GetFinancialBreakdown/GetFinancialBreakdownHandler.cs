using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using SFA.DAS.ManageApprenticeships.Models.Constants;

namespace SFA.DAS.ManageApprenticeships.Application.Queries.Transfers.GetFinancialBreakdown
{
    public class GetFinancialBreakdownHandler : IRequestHandler<GetFinancialBreakdownQuery, GetFinancialBreakdownResult>
    {
        readonly IForecastingApiClient<ForecastingApiConfiguration> _forecastingApiClient;
        readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _levyTransferMatchingApiClient;

        public GetFinancialBreakdownHandler(IForecastingApiClient<ForecastingApiConfiguration> forecastingApiClient, ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> levyTransferMatchingApiClient)
        {
            _forecastingApiClient = forecastingApiClient;
            _levyTransferMatchingApiClient = levyTransferMatchingApiClient;
        }

        public async Task<GetFinancialBreakdownResult> Handle(GetFinancialBreakdownQuery request, CancellationToken cancellationToken)
        {
            var transferFinancialBreakdownTask = _forecastingApiClient.Get<GetTransferFinancialBreakdownResponse>
                                                        (new GetTransferFinancialBreakdownRequest(request.AccountId));
            
            var pledgesTask = _levyTransferMatchingApiClient.Get<GetPledgesResponse>
                                                        (new GetPledgesRequest(request.AccountId));

            await Task.WhenAll(transferFinancialBreakdownTask, pledgesTask);

            return new GetFinancialBreakdownResult
            {
                Commitments = transferFinancialBreakdownTask.Result.Breakdown.Sum(x => x.FundsOut.Commitments),
                ApprovedPledgeApplications = transferFinancialBreakdownTask.Result.Breakdown.Sum(x => x.FundsOut.ApprovedPledgeApplications),
                TransferConnections = transferFinancialBreakdownTask.Result.Breakdown.Sum(x => x.FundsOut.TransferConnections),
                AcceptedPledgeApplications = transferFinancialBreakdownTask.Result.Breakdown.Sum(x => x.FundsOut.AcceptedPledgeApplications),
                PledgeOriginatedCommitments = transferFinancialBreakdownTask.Result.Breakdown.Sum(x => x.FundsOut.PledgeOriginatedCommitments),
                FundsIn = transferFinancialBreakdownTask.Result.Breakdown.Sum(x => x.FundsIn),
                ProjectionStartDate = transferFinancialBreakdownTask.Result.ProjectionStartDate,
                NumberOfMonths = transferFinancialBreakdownTask.Result.NumberOfMonths,
                AmountPledged = pledgesTask.Result.Pledges.Where(p => p.Status != PledgeStatus.Closed).Sum(x => x.Amount)
            };
        }
    }
}
