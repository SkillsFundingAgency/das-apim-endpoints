using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.EmployerAccounts.InnerApi.Responses;

public record GetPagedVacancySummaryApiResponse
{
    [JsonPropertyName("info")]
    public Info PageInfo { get; set; }
    [JsonPropertyName("items")]
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

public record VacancySummary
{
    public string Title { get; set; }
    public VacancyStatus Status { get; set; }
    public int? NoOfNewApplications { get; set; }
    public int? NoOfSuccessfulApplications { get; set; }
    public int? NoOfUnsuccessfulApplications { get; set; }
    public DateTime? ClosingDate { get; set; }
    public DateTime? ClosedDate { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VacancyStatus
{
    Draft,
    Review,
    Rejected,
    Submitted,
    Referred,
    Live,
    Closed,
    Approved
}
