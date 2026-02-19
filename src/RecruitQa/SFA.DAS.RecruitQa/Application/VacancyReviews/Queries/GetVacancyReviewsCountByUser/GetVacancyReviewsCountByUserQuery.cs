using MediatR;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsCountByUser;

public class GetVacancyReviewsCountByUserQuery : IRequest<GetVacancyReviewsCountByUserQueryResult>
{
    public required string UserEmail { get; set; }
    public bool? ApprovedFirstTime { get; set; }
    public DateTime? AssignationExpiry { get; set; }
}
