using SFA.DAS.Recruit.Application.Queries.GetVacancyMetrics;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Recruit.Api.Models
{
    public record GetVacancyMetricsResponse
    {
        public List<VacancyMetric> VacancyMetrics { get; set; } = [];

        public static implicit operator GetVacancyMetricsResponse(GetVacancyMetricsQueryResult source)
        {
            return new GetVacancyMetricsResponse
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

            public static implicit operator VacancyMetric(GetVacancyMetricsQueryResult.VacancyMetric source)
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