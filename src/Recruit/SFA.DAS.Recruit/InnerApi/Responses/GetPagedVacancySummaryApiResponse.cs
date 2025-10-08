using Newtonsoft.Json;
using SFA.DAS.Recruit.Domain;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Responses;
public record GetPagedVacancySummaryApiResponse
{
    [JsonProperty("info")]
    public Info PageInfo { get; set; }
    [JsonProperty("items")]
    public List<VacancySummary> Items { get; set; }

    public record Info
    {
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}