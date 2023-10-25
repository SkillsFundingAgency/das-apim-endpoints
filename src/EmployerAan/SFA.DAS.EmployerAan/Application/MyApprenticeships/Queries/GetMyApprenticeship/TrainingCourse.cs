namespace SFA.DAS.EmployerAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;

public class TrainingCourse
{
    public string? Name { get; set; }
    public int Level { get; set; }
    public int? Duration { get; set; }
    public string? Sector { get; set; }

    public static implicit operator TrainingCourse(GetStandardResponse standard)
    {
        return new TrainingCourse
        {
            Name = standard.Title,
            Level = standard.Level,
            Sector = standard.Route,
            Duration = standard?.VersionDetail?.ProposedTypicalDuration
        };
    }

    public static implicit operator TrainingCourse(GetFrameworkResponse framework)
    {
        return new TrainingCourse
        {
            Name = framework.Title,
            Level = framework.Level,
            Sector = framework.FrameworkName,
            Duration = framework.Duration
        };
    }
}