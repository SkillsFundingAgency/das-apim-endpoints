using System.Text.Json.Serialization;

namespace SFA.DAS.Recruit.Api.Models.Requests;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VacancySortColumn
{
    CreatedDate, // default sort column
    ClosingDate,
}