using System.Text.Json.Serialization;

namespace SFA.DAS.RecruitJobs.Domain.Vacancy;

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