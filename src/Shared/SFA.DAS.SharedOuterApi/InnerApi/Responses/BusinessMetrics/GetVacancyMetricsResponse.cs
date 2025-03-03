using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.BusinessMetrics
{
    public record GetVacancyMetricsResponse
    {
        [JsonProperty("vacancyMetrics")]
        public List<VacancyMetric> VacancyMetrics { get; set; } = [];

        public record VacancyMetric
        {
            [JsonProperty("vacancyReference")]
            public string? VacancyReference { get; init; }
            [JsonProperty("viewsCount")]
            public long ViewsCount { get; init; }
            [JsonProperty("searchResultsCount")]
            public long SearchResultsCount { get; init; }
            [JsonProperty("applicationStartedCount")]
            public long ApplicationStartedCount { get; init; }
            [JsonProperty("applicationSubmittedCount")]
            public long ApplicationSubmittedCount { get; init; }
        }
    }
}