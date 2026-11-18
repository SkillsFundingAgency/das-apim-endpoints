using SFA.DAS.Recruit.Contracts.ApiResponses;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByUser;

public class GetVacancyReviewsByUserQuery : IRequest<GetVacancyReviewsByUserQueryResult>
{
    public required string UserId { get; set; }
    public DateTime? AssignationExpiry { get; set; }
    public ReviewStatus? Status { get; set; }
}
