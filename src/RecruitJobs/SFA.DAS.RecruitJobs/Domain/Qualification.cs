using SFA.DAS.SharedOuterApi.Types.Domain.Recruit;

namespace SFA.DAS.RecruitJobs.Domain;

public class Qualification
{
    public string QualificationType { get; set; }
    public string Subject { get; set; }
    public string Grade { get; set; }
    public int? Level { get; set; }
    public QualificationWeighting? Weighting { get; set; }
    public string? OtherQualificationName { get; set; }
}