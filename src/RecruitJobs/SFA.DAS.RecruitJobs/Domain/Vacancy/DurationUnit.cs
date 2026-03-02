using System.Text.Json.Serialization;

namespace SFA.DAS.RecruitJobs.Domain.Vacancy;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DurationUnit
{
    Week,
    Month,
    Year
}