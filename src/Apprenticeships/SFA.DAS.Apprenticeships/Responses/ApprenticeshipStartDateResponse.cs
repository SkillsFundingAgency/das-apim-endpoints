namespace SFA.DAS.Apprenticeships.Responses;
public class ApprenticeshipStartDateResponse
{
    public Guid ApprenticeshipKey { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    public string? EmployerName { get; set; }
    public string? ProviderName { get; set; }
    public DateTime? EarliestStartDate { get; set; }
    public DateTime? LatestStartDate { get; set; }
    public DateTime LastFridayOfSchool { get; set; }
    public StandardInfo Standard { get; set; } = null!;
}

public class StandardInfo
{
    public string? CourseCode { get; set; } = null!;
    public DateTime? EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public StandardVersionInfo? StandardVersion { get; set; } = null!;
}

public class StandardVersionInfo
{
    public string Version { get; set; } = null!;
    public DateTime? VersionEarliestStartDate { get; set; }
    public DateTime? VersionLatestStartDate { get; set; }
}