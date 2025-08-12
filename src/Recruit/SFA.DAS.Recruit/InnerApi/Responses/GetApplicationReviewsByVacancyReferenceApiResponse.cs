using SFA.DAS.Recruit.Domain;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Responses
{
    public record GetApplicationReviewsByVacancyReferenceApiResponse
    {
        public List<ApplicationReview> ApplicationReviews { get; set; } = [];
    }
}
