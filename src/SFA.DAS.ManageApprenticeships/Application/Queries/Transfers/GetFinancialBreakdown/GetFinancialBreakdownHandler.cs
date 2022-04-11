using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ManageApprenticeships.Application.Queries.Transfers.GetFinancialBreakdown
{
    public class GetFinancialBreakdownHandler : IRequestHandler<GetFinancialBreakdownQuery, GetFinancialBreakdownResult>
    {
        readonly IForecastingApiClient<ForecastingApiConfiguration> _forecastingApiClient;

        public GetFinancialBreakdownHandler(IForecastingApiClient<ForecastingApiConfiguration> forecastingApiClient)
        {
            _forecastingApiClient = forecastingApiClient;
        }

        public async Task<GetFinancialBreakdownResult> Handle(GetFinancialBreakdownQuery request, CancellationToken cancellationToken)
        {
            var transferFinancialBreakdownTask = await _forecastingApiClient.Get<GetTransferFinancialBreakdownResponse>
                                                        (new GetTransferFinancialBreakdownRequest(request.AccountId));


            return new GetFinancialBreakdownResult
            {
                ProjectionStartDate = transferFinancialBreakdownTask.ProjectionStartDate
            };
        }
    }
}
