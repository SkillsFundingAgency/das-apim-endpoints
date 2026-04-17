using System.Text.Json.Serialization;

<<<<<<<< HEAD:src/Shared/SFA.DAS.SharedOuterApi.Types/Domain/Recruit/WageType.cs
namespace SFA.DAS.SharedOuterApi.Types.Domain.Recruit;
========
namespace SFA.DAS.RecruitJobs.Domain;
>>>>>>>> master:src/RecruitJobs/SFA.DAS.RecruitJobs/Domain/WageType.cs

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WageType
{
    FixedWage,
    NationalMinimumWageForApprentices,
    NationalMinimumWage,
    CompetitiveSalary,
    Unspecified
}