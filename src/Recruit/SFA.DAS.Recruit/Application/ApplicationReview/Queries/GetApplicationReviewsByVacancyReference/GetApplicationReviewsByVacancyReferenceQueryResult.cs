using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReference
{
    public record GetApplicationReviewsByVacancyReferenceQueryResult
    {
        public List<InnerApi.Responses.ApplicationReview> ApplicationReviews { get; init; } = new();
        public GetApplicationReviewsByVacancyReferenceQueryResult(List<InnerApi.Responses.ApplicationReview> applicationReviews)
        {
            ApplicationReviews = applicationReviews;
        }
    }
}
