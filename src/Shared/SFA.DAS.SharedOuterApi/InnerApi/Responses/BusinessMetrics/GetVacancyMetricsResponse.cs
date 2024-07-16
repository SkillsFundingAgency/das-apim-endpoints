using Newtonsoft.Json;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.BusinessMetrics
{
    public record GetVacancyMetricsResponse
    {
        [JsonProperty("ViewsCount")]
        public long ViewsCount { get; init; }
        [JsonProperty("SearchResultsCount")]
        public long SearchResultsCount { get; init; }
        [JsonProperty("ApplicationStartedCount")]
        public long ApplicationStartedCount { get; init; }
        [JsonProperty("ApplicationSubmittedCount")]
        public long ApplicationSubmittedCount { get; init; }
    }
}