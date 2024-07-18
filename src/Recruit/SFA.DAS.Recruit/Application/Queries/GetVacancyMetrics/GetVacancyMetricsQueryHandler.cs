using MediatR;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.BusinessMetrics;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.BusinessMetrics;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetVacancyMetrics
{
    public record GetVacancyMetricsQueryHandler : IRequestHandler<GetVacancyMetricsQuery, GetVacancyMetricsQueryResult>
    {
        private readonly IBusinessMetricsApiClient<BusinessMetricsConfiguration> _apiClient;

        public GetVacancyMetricsQueryHandler(IBusinessMetricsApiClient<BusinessMetricsConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetVacancyMetricsQueryResult> Handle(GetVacancyMetricsQuery request, CancellationToken cancellationToken)
        {
            var vacancyOuterApiResponseTask = _apiClient.Get<GetVacancyMetricsResponse>(new GetVacancyMetricsRequest(nameof(BusinessMetricServiceNames.VacanciesOuterApi), request.VacancyReference, request.StartDate, request.EndDate));
            var findAnApprenticeshipOuterApiResponseTask = _apiClient.Get<GetVacancyMetricsResponse>(new GetVacancyMetricsRequest(nameof(BusinessMetricServiceNames.FindAnApprenticeshipOuterApi), request.VacancyReference, request.StartDate, request.EndDate));

            await Task.WhenAll(vacancyOuterApiResponseTask, findAnApprenticeshipOuterApiResponseTask);

            if(vacancyOuterApiResponseTask.Result is null || findAnApprenticeshipOuterApiResponseTask.Result is null)
            {
                return new GetVacancyMetricsQueryResult();
            }

            return new GetVacancyMetricsQueryResult
            {
                ViewsCount = vacancyOuterApiResponseTask.Result.ViewsCount + findAnApprenticeshipOuterApiResponseTask.Result.ViewsCount,
                ApplicationStartedCount = vacancyOuterApiResponseTask.Result.ApplicationStartedCount + findAnApprenticeshipOuterApiResponseTask.Result.ApplicationStartedCount,
                ApplicationSubmittedCount = vacancyOuterApiResponseTask.Result.ApplicationSubmittedCount + findAnApprenticeshipOuterApiResponseTask.Result.ApplicationSubmittedCount,
                SearchResultsCount = vacancyOuterApiResponseTask.Result.SearchResultsCount + findAnApprenticeshipOuterApiResponseTask.Result.SearchResultsCount,
            };
        }
    }
}