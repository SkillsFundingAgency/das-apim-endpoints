namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByFilter;

public class GetVacancyReviewsByFilterQuery : IRequest<GetVacancyReviewsByFilterQueryResult>
{
    public List<Recruit.Contracts.ApiResponses.ReviewStatus>? Status { get; set; }
    public DateTime? ExpiredAssignationDateTime { get; set; }
}
