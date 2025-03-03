using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.BusinessMetrics;

namespace SFA.DAS.Recruit.Application.Queries.GetVacancyMetrics
{
    public record GetVacancyMetricsQueryResult
    {
        public List<VacancyMetric> VacancyMetrics { get; set; } = [];

        public static implicit operator GetVacancyMetricsQueryResult(GetVacancyMetricsResponse source)
        {
            return new GetVacancyMetricsQueryResult
            {
                VacancyMetrics = source.VacancyMetrics.Select(x => (VacancyMetric)x).ToList()
            };
        }

        public record VacancyMetric
        {
            public string? VacancyReference { get; init; }
            public long ViewsCount { get; init; }
            public long SearchResultsCount { get; init; }
            public long ApplicationStartedCount { get; init; }
            public long ApplicationSubmittedCount { get; init; }

            public static implicit operator VacancyMetric(GetVacancyMetricsResponse.VacancyMetric source)
            {
                return new VacancyMetric
                {
                    VacancyReference = source.VacancyReference,
                    ApplicationStartedCount = source.ApplicationStartedCount,
                    ApplicationSubmittedCount = source.ApplicationSubmittedCount,
                    SearchResultsCount = source.SearchResultsCount,
                    ViewsCount = source.ViewsCount
                };
            }
        }
    }
}
