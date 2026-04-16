namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewByVacancyReferenceAndCandidateId;

public record GetApplicationReviewByVacancyReferenceAndCandidateIdQueryResult
{
    public Domain.ApplicationReview? ApplicationReview { get; init; }
}