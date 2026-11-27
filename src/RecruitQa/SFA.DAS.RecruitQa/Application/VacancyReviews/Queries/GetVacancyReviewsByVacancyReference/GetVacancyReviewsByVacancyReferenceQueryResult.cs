using SFA.DAS.Recruit.Contracts.ApiResponses;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByVacancyReference;

public sealed record GetVacancyReviewsByVacancyReferenceQueryResult
{
    public required List<VacancyReview> VacancyReviews { get; set; }
}