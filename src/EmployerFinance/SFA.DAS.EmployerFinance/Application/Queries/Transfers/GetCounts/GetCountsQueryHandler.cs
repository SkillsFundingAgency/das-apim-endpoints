using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetCounts
{
    public class GetCountsQueryHandler : IRequestHandler<GetCountsQuery, GetCountsQueryResult>
    {
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _levyTransferMatchingApiClient;
        private readonly IForecastingApiClient<ForecastingApiConfiguration> _forecastingApiClient;

        public GetCountsQueryHandler(ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> levyTransferMatchingApiClient,
            IForecastingApiClient<ForecastingApiConfiguration> forecastingApiClient)
        {
            _levyTransferMatchingApiClient = levyTransferMatchingApiClient;
            _forecastingApiClient = forecastingApiClient;
        }

        public async Task<GetCountsQueryResult> Handle(GetCountsQuery request, CancellationToken cancellationToken)
        {
            var pledgesTask = _levyTransferMatchingApiClient.Get<GetPledgesResponse>(new GetPledgesRequest(request.AccountId));

            var applicationsTask = _levyTransferMatchingApiClient.Get<GetApplicationsResponse>(new GetApplicationsRequest
            {
                AccountId = request.AccountId
            });

            var breakdownTask = _forecastingApiClient.Get<GetTransferFinancialBreakdownResponse>
                (new GetTransferFinancialBreakdownRequest(request.AccountId, DateTime.UtcNow));

            await Task.WhenAll(breakdownTask, pledgesTask, applicationsTask);

            return new GetCountsQueryResult
            {
                PledgesCount = pledgesTask.Result.TotalPledges,
                ApplicationsCount = applicationsTask.Result.Applications.Count(),
                CurrentYearEstimatedCommittedSpend = CalculateEstimatedCommittedSpend(breakdownTask.Result.Breakdown),
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
