namespace SFA.DAS.Recruit.InnerApi.Responses;

public record GetDashboardApiResponse
{
    public int NewApplicationsCount { get; set; } = 0;
    public int EmployerReviewedApplicationsCount { get; set; } = 0;
    public int SharedApplicationsCount { get; set; } = 0;
    public int AllSharedApplicationsCount { get; set; } = 0;
    public int SuccessfulApplicationsCount { get; set; } = 0;
    public int UnsuccessfulApplicationsCount { get; set; } = 0;
    public bool HasNoApplications { get; set; } = false;
    public int ClosedVacanciesCount { get; set; } = 0;
    public int DraftVacanciesCount { get; set; } = 0;
    public int ReviewVacanciesCount { get; set; } = 0;
    public int ReferredVacanciesCount { get; set; } = 0;
    public int LiveVacanciesCount { get; set; } = 0;
    public int SubmittedVacanciesCount { get; set; } = 0;
    public int ClosingSoonVacanciesCount { get; set; } = 0;
    public int ClosingSoonWithNoApplications { get; set; } = 0;
}