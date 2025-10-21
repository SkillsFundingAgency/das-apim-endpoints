using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReferenceAndTempStatus;
public record GetApplicationReviewsByVacancyReferenceAndTempStatusQueryResult(
    List<Domain.ApplicationReview> ApplicationReviews)
{
    public List<Domain.ApplicationReview> ApplicationReviews { get; init; } = ApplicationReviews;
}