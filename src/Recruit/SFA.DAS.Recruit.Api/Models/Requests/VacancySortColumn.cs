using System.Text.Json.Serialization;

namespace SFA.DAS.Recruit.Api.Models.Requests;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VacancySortColumn
{
    ClosingDate, // default sort column
    Id,
    VacancyReference,
}