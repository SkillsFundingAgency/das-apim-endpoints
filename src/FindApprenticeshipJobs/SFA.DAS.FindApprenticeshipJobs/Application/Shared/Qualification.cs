namespace SFA.DAS.FindApprenticeshipJobs.Application.Shared;
public class Qualification
{
    public string QualificationType { get; set; }
    public string Subject { get; set; }
    public string Grade { get; set; }
    public QualificationWeighting? Weighting { get; set; }
}

public enum QualificationWeighting
{
    Essential,
    Desired
}
