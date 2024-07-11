using SFA.DAS.Recruit.Application.Queries.GetVacancyMetrics;

namespace SFA.DAS.Recruit.Api.Models
{
    public record GetVacancyMetricsResponse
    {
        public long ViewsCount { get; init; }
        public long SearchResultsCount { get; init; }
        public long ApplicationStartedCount { get; init; }
        public long ApplicationSubmittedCount { get; init; }

        public static implicit operator GetVacancyMetricsResponse(GetVacancyMetricsQueryResult source)
        {
            return new GetVacancyMetricsResponse
            {
                ViewsCount = source.ViewsCount,
                ApplicationStartedCount = source.ApplicationStartedCount,
                ApplicationSubmittedCount = source.ApplicationSubmittedCount,
                SearchResultsCount = source.SearchResultsCount
            };
        }
    }
}