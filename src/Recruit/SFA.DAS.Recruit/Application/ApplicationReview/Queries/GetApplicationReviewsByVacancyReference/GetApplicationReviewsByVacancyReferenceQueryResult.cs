using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReference
{
    public record GetApplicationReviewsByVacancyReferenceQueryResult
    {
        public List<Domain.ApplicationReview> ApplicationReviews { get; init; } = new();
        public GetApplicationReviewsByVacancyReferenceQueryResult(List<Domain.ApplicationReview> applicationReviews)
        {
            ApplicationReviews = applicationReviews;
        }
    }
}
