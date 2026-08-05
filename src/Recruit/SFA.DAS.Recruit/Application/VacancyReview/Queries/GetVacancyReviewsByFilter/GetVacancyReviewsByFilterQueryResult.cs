using System.Collections.Generic;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;

namespace SFA.DAS.Recruit.Application.VacancyReview.Queries.GetVacancyReviewsByFilter;

public class GetVacancyReviewsByFilterQueryResult
{
    public required List<GetVacancyReviewResponse> VacancyReviews { get; set; }
}
