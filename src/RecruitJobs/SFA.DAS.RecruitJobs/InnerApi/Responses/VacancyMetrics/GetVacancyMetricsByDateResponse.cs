using System.Collections.Generic;

namespace SFA.DAS.RecruitJobs.InnerApi.Responses.VacancyMetrics;

public sealed record GetVacancyMetricsByDateResponse
{
    public List<VacancyMetric> VacancyMetrics { get; set; } = [];

    public record VacancyMetric
    {
        public string? VacancyReference { get; init; }
        public long ViewsCount { get; init; }
        public long SearchResultsCount { get; init; }
        public long ApplicationStartedCount { get; init; }
        public long ApplicationSubmittedCount { get; init; }
    }
}