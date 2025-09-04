using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Responses;

public class GetVacancyReviewsListResponse
{
    public List<GetVacancyReviewResponse> VacancyReviews { get; set; }
}