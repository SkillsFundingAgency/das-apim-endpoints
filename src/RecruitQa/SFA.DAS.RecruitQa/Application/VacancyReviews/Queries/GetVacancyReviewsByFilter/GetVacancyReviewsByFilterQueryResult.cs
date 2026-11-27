using SFA.DAS.Recruit.Contracts.ApiResponses;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByFilter;

public sealed record GetVacancyReviewsByFilterQueryResult
{
    public required List<VacancyReview> VacancyReviews { get; init; }
}