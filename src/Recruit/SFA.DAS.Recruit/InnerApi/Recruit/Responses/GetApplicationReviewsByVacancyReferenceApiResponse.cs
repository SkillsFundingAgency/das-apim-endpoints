using System.Collections.Generic;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Responses
{
    public record GetApplicationReviewsByVacancyReferenceApiResponse
    {
        public List<ApplicationReview> ApplicationReviews { get; set; } = [];
    }
}
