using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Types.Domain.Domain.Recruit;

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