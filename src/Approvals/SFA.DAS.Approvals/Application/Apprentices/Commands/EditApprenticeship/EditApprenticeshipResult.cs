namespace SFA.DAS.Approvals.Application.Apprentices.Commands.EditApprenticeship;

public class EditApprenticeshipResult
{
    public long ApprenticeshipId { get; set; }
    public bool HasOptions { get; set; }
    public string Version { get; set; }
    public bool CourseOrStartDateChanged { get; set; }
}