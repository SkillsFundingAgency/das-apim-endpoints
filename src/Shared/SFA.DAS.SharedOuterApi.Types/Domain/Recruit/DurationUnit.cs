using System.Text.Json.Serialization;

<<<<<<<< HEAD:src/Shared/SFA.DAS.SharedOuterApi.Types/Domain/Recruit/DurationUnit.cs
namespace SFA.DAS.SharedOuterApi.Types.Domain.Recruit;
========
namespace SFA.DAS.RecruitJobs.Domain;
>>>>>>>> master:src/RecruitJobs/SFA.DAS.RecruitJobs/Domain/DurationUnit.cs

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DurationUnit
{
    Week,
    Month,
    Year
}