using System.Text.Json.Serialization;

namespace SFA.DAS.FindApprenticeshipJobs.Domain.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum WageType
    {
        Unspecified = 1,
        NationalMinimumWageForApprentices = 2,
        NationalMinimumWage = 3,
        FixedWage = 4,
        CompetitiveSalary = 6,
    }
}
