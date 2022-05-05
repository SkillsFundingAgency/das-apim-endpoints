using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using SFA.DAS.ManageApprenticeships.Models.Constants;
using System;
using System.Collections.Generic;

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
            var breakdownTask = _forecastingApiClient.Get<GetTransferFinancialBreakdownResponse>
                                                        (new GetTransferFinancialBreakdownRequest(request.AccountId, DateTime.UtcNow));

            var nextYearBreakdowntask = _forecastingApiClient.Get<GetTransferFinancialBreakdownResponse>
                                                        (new GetTransferFinancialBreakdownRequest(request.AccountId, DateTime.UtcNow.AddYears(1)));

            var yearAfterNextYearBreakdowntask = _forecastingApiClient.Get<GetTransferFinancialBreakdownResponse>
                                                        (new GetTransferFinancialBreakdownRequest(request.AccountId, DateTime.UtcNow.AddYears(2)));

            var pledgesTask = _levyTransferMatchingApiClient.Get<GetPledgesResponse>
                                                        (new GetPledgesRequest(request.AccountId));

            await Task.WhenAll(breakdownTask, nextYearBreakdowntask, yearAfterNextYearBreakdowntask, pledgesTask);

            return new GetFinancialBreakdownResult
            {
                Commitments = breakdownTask.Result.Breakdown.Sum(x => x.FundsOut.Commitments),
                ApprovedPledgeApplications = breakdownTask.Result.Breakdown.Sum(x => x.FundsOut.ApprovedPledgeApplications),
                TransferConnections = breakdownTask.Result.Breakdown.Sum(x => x.FundsOut.TransferConnections),
                AcceptedPledgeApplications = breakdownTask.Result.Breakdown.Sum(x => x.FundsOut.AcceptedPledgeApplications),
                PledgeOriginatedCommitments = breakdownTask.Result.Breakdown.Sum(x => x.FundsOut.PledgeOriginatedCommitments),                
                ProjectionStartDate = breakdownTask.Result.ProjectionStartDate,
                CurrentYearEstimatedCommittedSpend = CalculateEstimatedCommittedSpend(breakdownTask.Result.Breakdown),
                NextYearEstimatedCommittedSpend = CalculateEstimatedCommittedSpend(nextYearBreakdowntask.Result.Breakdown),
                YearAfterNextYearEstimatedCommittedSpend = CalculateEstimatedCommittedSpend(yearAfterNextYearBreakdowntask.Result.Breakdown),
                AmountPledged = pledgesTask.Result.Pledges.Where(p => p.Status != PledgeStatus.Closed).Sum(x => x.Amount)
            };
        }

        private decimal CalculateEstimatedCommittedSpend(List<GetTransferFinancialBreakdownResponse.BreakdownDetails> breakdown)
        {
            return breakdown.Sum(x => x.FundsOut.ApprovedPledgeApplications) +
                   breakdown.Sum(x => x.FundsOut.AcceptedPledgeApplications) +
                   breakdown.Sum(x => x.FundsOut.PledgeOriginatedCommitments) +
                   breakdown.Sum(x => x.FundsOut.TransferConnections);
        }
    }
}
