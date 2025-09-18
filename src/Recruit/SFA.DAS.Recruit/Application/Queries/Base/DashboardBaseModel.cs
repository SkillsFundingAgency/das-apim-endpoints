using System;

namespace SFA.DAS.Recruit.Application.Queries.Base;
public record DashboardBaseModel
{
    public int NewApplicationsCount { get; init; }
    public int EmployerReviewedApplicationsCount { get; init; }
    public int SharedApplicationsCount { get; init; }
    public int AllSharedApplicationsCount { get; init; }
    public int SuccessfulApplicationsCount { get; init; }
    public int UnsuccessfulApplicationsCount { get; init; }
    public bool HasNoApplications { get; init; }

    public int ClosedVacanciesCount { get; init; }
    public int DraftVacanciesCount { get; init; }
    public int ReviewVacanciesCount { get; init; }
    public int ReferredVacanciesCount { get; init; }
    public int LiveVacanciesCount { get; init; }
    public int SubmittedVacanciesCount { get; init; }
    public int ClosingSoonVacanciesCount { get; init; }
    public int ClosingSoonWithNoApplications { get; init; }

    protected static T Map<T>(InnerApi.Responses.GetDashboardApiResponse response, Func<T> factory)
        where T : DashboardBaseModel
    {
        var model = factory();

        return model with
        {
            NewApplicationsCount = response.NewApplicationsCount,
            EmployerReviewedApplicationsCount = response.EmployerReviewedApplicationsCount,
            SharedApplicationsCount = response.SharedApplicationsCount,
            AllSharedApplicationsCount = response.AllSharedApplicationsCount,
            SuccessfulApplicationsCount = response.SuccessfulApplicationsCount,
            UnsuccessfulApplicationsCount = response.UnsuccessfulApplicationsCount,
            HasNoApplications = response.HasNoApplications,
            ClosedVacanciesCount = response.ClosedVacanciesCount,
            DraftVacanciesCount = response.DraftVacanciesCount,
            ReviewVacanciesCount = response.ReviewVacanciesCount,
            ReferredVacanciesCount = response.ReferredVacanciesCount,
            LiveVacanciesCount = response.LiveVacanciesCount,
            SubmittedVacanciesCount = response.SubmittedVacanciesCount,
            ClosingSoonVacanciesCount = response.ClosingSoonVacanciesCount,
            ClosingSoonWithNoApplications = response.ClosingSoonWithNoApplications,
        };
    }
}
