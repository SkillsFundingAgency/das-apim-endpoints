using MediatR;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByFilter;

public class GetVacancyReviewsByFilterQuery : IRequest<GetVacancyReviewsByFilterQueryResult>
{
    public List<string>? Status { get; set; }
    public DateTime? ExpiredAssignationDateTime { get; set; }
}
