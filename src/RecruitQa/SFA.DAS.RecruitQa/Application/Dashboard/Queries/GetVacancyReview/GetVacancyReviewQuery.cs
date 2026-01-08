using MediatR;

namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReview;

public class GetVacancyReviewQuery : IRequest<GetVacancyReviewQueryResult>
{
    public Guid Id { get; set; }
}