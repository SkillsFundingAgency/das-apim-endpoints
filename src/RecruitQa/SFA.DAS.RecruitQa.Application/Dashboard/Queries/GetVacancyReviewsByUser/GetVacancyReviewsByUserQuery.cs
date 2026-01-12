using MediatR;

namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsByUser;

public class GetVacancyReviewsByUserQuery : IRequest<GetVacancyReviewsByUserQueryResult>
{
    public required string UserId { get; set; }
    public DateTime? AssignationExpiry { get; set; }
}
