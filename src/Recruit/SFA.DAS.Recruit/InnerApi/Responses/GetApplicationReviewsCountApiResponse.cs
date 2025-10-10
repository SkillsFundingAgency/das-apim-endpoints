namespace SFA.DAS.Recruit.InnerApi.Responses;
public record GetApplicationReviewsCountApiResponse
{
    public long VacancyReference { get; init; } = 0;
    public int Applications { get; init; } = 0;
    public int NewApplications { get; init; } = 0;
    public int SharedApplications { get; init; } = 0;
    public int AllSharedApplications { get; init; } = 0;
    public int SuccessfulApplications { get; init; } = 0;
    public int UnsuccessfulApplications { get; init; } = 0;
    public int EmployerReviewedApplications { get; init; } = 0;
    public int InterviewingApplications { get; init; } = 0;
    public int InReviewApplications { get; init; } = 0;
    public int EmployerInterviewingApplications { get; init; } = 0;
    public bool HasNoApplications { get; init; } = false;
}