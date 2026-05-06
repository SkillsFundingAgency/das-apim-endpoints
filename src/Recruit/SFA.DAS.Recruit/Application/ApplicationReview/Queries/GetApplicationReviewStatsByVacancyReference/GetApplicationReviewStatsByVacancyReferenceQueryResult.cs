using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewStatsByVacancyReference;
public record GetApplicationReviewStatsByVacancyReferenceQueryResult
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

    public static GetApplicationReviewStatsByVacancyReferenceQueryResult FromApplicationReviewsCountResponse(GetApplicationReviewsCountApiResponse response)
    {
        return new GetApplicationReviewStatsByVacancyReferenceQueryResult
        {
            AllSharedApplications = response.AllSharedApplications,
            Applications = response.Applications,
            EmployerInterviewingApplications = response.EmployerInterviewingApplications,
            EmployerReviewedApplications = response.EmployerReviewedApplications,
            HasNoApplications = response.HasNoApplications,
            InReviewApplications = response.InReviewApplications,
            InterviewingApplications = response.InterviewingApplications,
            NewApplications = response.NewApplications,
            SharedApplications = response.SharedApplications,
            SuccessfulApplications = response.SuccessfulApplications,
            UnsuccessfulApplications = response.UnsuccessfulApplications,
            VacancyReference = response.VacancyReference,
        };
    }
}
