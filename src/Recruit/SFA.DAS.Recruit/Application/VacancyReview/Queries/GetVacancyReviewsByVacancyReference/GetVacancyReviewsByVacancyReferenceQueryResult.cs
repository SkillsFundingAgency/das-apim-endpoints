using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.VacancyReview.Queries.GetVacancyReviewsByVacancyReference;

public class GetVacancyReviewsByVacancyReferenceQueryResult
{
    public required List<GetVacancyReviewResponse> VacancyReviews { get; set; }
}
