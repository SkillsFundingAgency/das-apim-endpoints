namespace SFA.DAS.Apprenticeships.Responses;

public class StandardVersionInfo
{
    public string Version { get; set; } = null!;
    public DateTime? VersionEarliestStartDate { get; set; }
    public DateTime? VersionLatestStartDate { get; set; }
}