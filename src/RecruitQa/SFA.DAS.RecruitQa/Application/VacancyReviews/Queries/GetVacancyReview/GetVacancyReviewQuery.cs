using MediatR;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReview;

public class GetVacancyReviewQuery : IRequest<GetVacancyReviewQueryResult>
{
    public Guid Id { get; set; }
}