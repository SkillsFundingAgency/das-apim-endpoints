using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReferenceAndStatus;
public record GetApplicationReviewsByVacancyReferenceAndStatusQueryResult(
    List<Domain.ApplicationReview> ApplicationReviews)
{
    public List<Domain.ApplicationReview> ApplicationReviews { get; init; } = ApplicationReviews;
}