using System.Text.Json.Serialization;

namespace SFA.DAS.RecruitQa.Domain;

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