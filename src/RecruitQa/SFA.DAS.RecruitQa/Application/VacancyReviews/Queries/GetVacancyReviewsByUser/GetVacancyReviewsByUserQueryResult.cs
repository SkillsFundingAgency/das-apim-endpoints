using SFA.DAS.Recruit.Contracts.ApiResponses;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByUser;

public sealed record GetVacancyReviewsByUserQueryResult
{
    public required List<VacancyReview> VacancyReviews { get; init; }
}