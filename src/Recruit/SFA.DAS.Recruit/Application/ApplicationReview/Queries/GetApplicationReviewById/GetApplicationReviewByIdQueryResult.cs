namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewById;
public record GetApplicationReviewByIdQueryResult
{
    public Domain.ApplicationReview ApplicationReview { get; init; }
}