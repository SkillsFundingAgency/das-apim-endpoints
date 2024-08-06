using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Requests;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetAccountProjectionSummary
{
    public class GetAccountProjectionSummaryQueryHandler : IRequestHandler<GetAccountProjectionSummaryQuery, GetAccountProjectionSummaryQueryResult>
    {
        private readonly IForecastingApiClient<ForecastingApiConfiguration> _forecastingApiClient;
        private readonly IFinanceApiClient<FinanceApiConfiguration> _financeApiClient;

        public GetAccountProjectionSummaryQueryHandler(IForecastingApiClient<ForecastingApiConfiguration> forecastingApiClient, 
            IFinanceApiClient<FinanceApiConfiguration> financeApiClient)
        {
            _forecastingApiClient = forecastingApiClient;
            _financeApiClient = financeApiClient;
        }

        public async Task<GetAccountProjectionSummaryQueryResult> Handle(GetAccountProjectionSummaryQuery request, CancellationToken cancellationToken)
        {
            var projectionCalcTask = _forecastingApiClient.Get<GetProjectionCalculationResponse>(new GetProjectionCalculationRequest(request.AccountId));
            var expiringFundsTask = _forecastingApiClient.Get<GetExpiringFundsResponse>(new GetExpiringFundsRequest(request.AccountId));
            var summaryFromFinanceTask = _financeApiClient.Get<GetAccountProjectionSummaryFromFinanceResponse>(
                new GetAccountProjectionSummaryFromFinanceRequest(request.AccountId));

            await Task.WhenAll(projectionCalcTask, expiringFundsTask, summaryFromFinanceTask);

            return new GetAccountProjectionSummaryQueryResult
            {
                AccountId = request.AccountId,
                FundsIn = summaryFromFinanceTask.Result?.FundsIn ?? 0,
                FundsOut = projectionCalcTask.Result?.FundsOut ?? 0,
                NumberOfMonths = projectionCalcTask.Result?.NumberOfMonths ?? 0,
                ProjectionGenerationDate = projectionCalcTask.Result?.ProjectionGenerationDate,
                ExpiryAmounts = expiringFundsTask.Result?.ExpiryAmounts ?? new List<GetExpiringFundsListItem>()
            };
        }
    }
}
