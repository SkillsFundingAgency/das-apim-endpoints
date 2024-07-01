﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Finance;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses.Finance;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetPledges
{
    public class GetPledgesQueryHandler : IRequestHandler<GetPledgesQuery, GetPledgesQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly IFinanceApiClient<FinanceApiConfiguration> _financeApiClient;
        private readonly IForecastingApiClient<ForecastingApiConfiguration> _forecastingApiClient;

        public GetPledgesQueryHandler(ILevyTransferMatchingService levyTransferMatchingService,
            IForecastingApiClient<ForecastingApiConfiguration> forecastingApiClient,
            IFinanceApiClient<FinanceApiConfiguration> financeApiClient)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _financeApiClient = financeApiClient;
            _forecastingApiClient = forecastingApiClient;
        }

        public async Task<GetPledgesQueryResult> Handle(GetPledgesQuery request, CancellationToken cancellationToken)
        {
            var ltmTask = _levyTransferMatchingService.GetPledges(new GetPledgesRequest(request.AccountId, page: request.Page, pageSize: request.PageSize));

            var fundingTask = _financeApiClient.Get<GetTransferAllowanceResponse>(new GetTransferAllowanceByAccountIdRequest(request.AccountId));

            var breakdownTask = _forecastingApiClient.Get<GetTransferFinancialBreakdownResponse>
               (new GetTransferFinancialBreakdownRequest(request.AccountId, DateTime.UtcNow));

            await Task.WhenAll(ltmTask, fundingTask, breakdownTask);

            var ltmResponse = await ltmTask;
            var fundingResponse = await fundingTask;
            var breakdownResponse = await breakdownTask;

            return new GetPledgesQueryResult
            {
                StartingTransferAllowance = fundingResponse.StartingTransferAllowance ?? 0,
                CurrentYearEstimatedCommittedSpend = CalculateEstimatedCommittedSpend(breakdownResponse.Breakdown),
                Pledges = ltmResponse.Pledges.Select(x => new GetPledgesQueryResult.Pledge
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    RemainingAmount = x.RemainingAmount,
                    ApplicationCount = x.ApplicationCount,
                    Status = x.Status
                }),
                TotalPledges = ltmResponse.TotalPledges,
                TotalPages = ltmResponse.TotalPages,
                Page = ltmResponse.Page,
                PageSize = ltmResponse.PageSize
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
