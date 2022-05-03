﻿using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using SFA.DAS.ManageApprenticeships.Models.Constants;
using System;

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
            var breakdownTask = await _forecastingApiClient.Get<GetTransferFinancialBreakdownResponse>
                                                        (new GetTransferFinancialBreakdownRequest(request.AccountId, DateTime.UtcNow));

            var nextYearBreakdowntask = await _forecastingApiClient.Get<GetTransferFinancialBreakdownResponse>
                                                        (new GetTransferFinancialBreakdownRequest(request.AccountId, DateTime.UtcNow.AddYears(1)));

            var yearAfterNextYearBreakdowntask = await _forecastingApiClient.Get<GetTransferFinancialBreakdownResponse>
                                                        (new GetTransferFinancialBreakdownRequest(request.AccountId, DateTime.UtcNow.AddYears(2)));
            var pledgesTask = await _levyTransferMatchingApiClient.Get<GetPledgesResponse>
                                                        (new GetPledgesRequest(request.AccountId));
            return new GetFinancialBreakdownResult
            {
                Commitments = breakdownTask.Breakdown.Sum(x => x.FundsOut.Commitments),
                ApprovedPledgeApplications = breakdownTask.Breakdown.Sum(x => x.FundsOut.ApprovedPledgeApplications),
                TransferConnections = breakdownTask.Breakdown.Sum(x => x.FundsOut.TransferConnections),
                AcceptedPledgeApplications = breakdownTask.Breakdown.Sum(x => x.FundsOut.AcceptedPledgeApplications),
                PledgeOriginatedCommitments = breakdownTask.Breakdown.Sum(x => x.FundsOut.PledgeOriginatedCommitments),                
                FundsIn = breakdownTask.Breakdown.Sum(x => x.FundsIn),
                ProjectionStartDate = breakdownTask.ProjectionStartDate,
                NumberOfMonths = breakdownTask.NumberOfMonths,
                CurrentYearEstimatedCommittedSpend = (breakdownTask.Breakdown.Sum(x => x.FundsOut.ApprovedPledgeApplications) +
                breakdownTask.Breakdown.Sum(x => x.FundsOut.AcceptedPledgeApplications) +
                breakdownTask.Breakdown.Sum(x => x.FundsOut.PledgeOriginatedCommitments)+ 
                breakdownTask.Breakdown.Sum(x => x.FundsOut.TransferConnections)),
                NextYearEstimatedCommittedSpend = (nextYearBreakdowntask.Breakdown.Sum(x => x.FundsOut.ApprovedPledgeApplications) +
                nextYearBreakdowntask.Breakdown.Sum(x => x.FundsOut.AcceptedPledgeApplications) +
                nextYearBreakdowntask.Breakdown.Sum(x => x.FundsOut.PledgeOriginatedCommitments) +
                nextYearBreakdowntask.Breakdown.Sum(x => x.FundsOut.TransferConnections)),
                YearAfterNextYearEstimatedCommittedSpend = (yearAfterNextYearBreakdowntask.Breakdown.Sum(x => x.FundsOut.ApprovedPledgeApplications) +
                yearAfterNextYearBreakdowntask.Breakdown.Sum(x => x.FundsOut.AcceptedPledgeApplications) +
                yearAfterNextYearBreakdowntask.Breakdown.Sum(x => x.FundsOut.PledgeOriginatedCommitments) +
                yearAfterNextYearBreakdowntask.Breakdown.Sum(x => x.FundsOut.TransferConnections)),
                AmountPledged = pledgesTask.Pledges.Where(p => p.Status != PledgeStatus.Closed).Sum(x => x.Amount)
            };
        }
    }
}
