namespace SFA.DAS.Recruit.Domain.Vacancy;

public class Qualification
{
    public string QualificationType { get; set; }
    public string Subject { get; set; }
    public string Grade { get; set; }
    public int? Level { get; set; }
    public QualificationWeighting? Weighting { get; set; }
    public string OtherQualificationName { get; set; }
}