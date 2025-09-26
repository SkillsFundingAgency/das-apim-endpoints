using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Requests;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetAccountProjectionSummary;

public class GetAccountProjectionSummaryQueryHandler(
    IForecastingApiClient<ForecastingApiConfiguration> forecastingApiClient,
    IFinanceApiClient<FinanceApiConfiguration> financeApiClient)
    : IRequestHandler<GetAccountProjectionSummaryQuery, GetAccountProjectionSummaryQueryResult>
{
    public async Task<GetAccountProjectionSummaryQueryResult> Handle(GetAccountProjectionSummaryQuery request, CancellationToken cancellationToken)
    {
        var projectionCalcTask = forecastingApiClient.Get<GetProjectionCalculationResponse>(new GetProjectionCalculationRequest(request.AccountId));
        var summaryFromFinanceTask = financeApiClient.Get<GetAccountProjectionSummaryFromFinanceResponse>(new GetAccountProjectionSummaryFromFinanceRequest(request.AccountId));

        await Task.WhenAll(projectionCalcTask, summaryFromFinanceTask);

        return new GetAccountProjectionSummaryQueryResult
        {
            AccountId = request.AccountId,
            FundsIn = summaryFromFinanceTask.Result?.FundsIn ?? 0,
            FundsOut = projectionCalcTask.Result?.FundsOut ?? 0,
            NumberOfMonths = projectionCalcTask.Result?.NumberOfMonths ?? 0,
            ProjectionGenerationDate = projectionCalcTask.Result?.ProjectionGenerationDate,
        };
    }
}