using MediatR;

namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsCountByUser;

public class GetVacancyReviewsCountByUserQuery : IRequest<GetVacancyReviewsCountByUserQueryResult>
{
    public required string UserId { get; set; }
    public bool? ApprovedFirstTime { get; set; }
}
