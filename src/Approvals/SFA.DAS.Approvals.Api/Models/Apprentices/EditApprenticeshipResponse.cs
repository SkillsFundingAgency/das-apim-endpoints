namespace SFA.DAS.Approvals.Api.Models.Apprentices;

public class EditApprenticeshipResponse
{
    public long ApprenticeshipId { get; set; }
    public bool HasOptions { get; set; }
    public bool CourseOrStartDateChange { get; set; }
    public string Version { get; set; }
}