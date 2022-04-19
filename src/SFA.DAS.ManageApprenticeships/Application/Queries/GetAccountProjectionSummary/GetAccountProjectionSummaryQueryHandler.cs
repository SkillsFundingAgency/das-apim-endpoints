using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ManageApprenticeships.InnerApi.Requests;
using SFA.DAS.ManageApprenticeships.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ManageApprenticeships.Application.Queries.GetAccountProjectionSummary
{
    public class GetAccountProjectionSummaryQueryHandler : IRequestHandler<GetAccountProjectionSummaryQuery, GetAccountProjectionSummaryQueryResult>
    {
        private readonly IForecastingApiClient<ForecastingApiConfiguration> _forecastingApiClient;

        public GetAccountProjectionSummaryQueryHandler(IForecastingApiClient<ForecastingApiConfiguration> forecastingApiClient)
        {
            _forecastingApiClient = forecastingApiClient;
        }

        public async Task<GetAccountProjectionSummaryQueryResult> Handle(GetAccountProjectionSummaryQuery request, CancellationToken cancellationToken)
        {
            var projectionCalcTask = _forecastingApiClient.Get<GetProjectionCalculationResponse>(new GetProjectionCalculationRequest(request.AccountId));
            var expiringFundsTask = _forecastingApiClient.Get<GetExpiringFundsResponse>(new GetExpiringFundsRequest(request.AccountId));

            await Task.WhenAll(projectionCalcTask, expiringFundsTask);

            return new GetAccountProjectionSummaryQueryResult
            {
                AccountId = projectionCalcTask.Result.AccountId,
                FundsIn = projectionCalcTask.Result.FundsIn,
                FundsOut = projectionCalcTask.Result.FundsOut,
                NumberOfMonths = projectionCalcTask.Result.NumberOfMonths,
                ProjectionGenerationDate = projectionCalcTask.Result.ProjectionGenerationDate,
                ExpiryAmounts = expiringFundsTask.Result.ExpiryAmounts
            };
        }
    }
}
