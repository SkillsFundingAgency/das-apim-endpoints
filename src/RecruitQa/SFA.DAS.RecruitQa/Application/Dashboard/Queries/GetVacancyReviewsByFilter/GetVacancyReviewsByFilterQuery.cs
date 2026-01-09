using MediatR;

namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsByFilter;

public class GetVacancyReviewsByFilterQuery : IRequest<GetVacancyReviewsByFilterQueryResult>
{
    public string? Status { get; set; }
    public DateTime? ExpiredAssignationDateTime { get; set; }
}
