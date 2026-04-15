using System.Text.Json.Serialization;

namespace SFA.DAS.RecruitJobs.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WageType
{
    FixedWage,
    NationalMinimumWageForApprentices,
    NationalMinimumWage,
    CompetitiveSalary,
    Unspecified
}