using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Responses;

public class GetVacancyReviewsListResponse
{
    public List<GetVacancyReviewResponse> VacancyReviews { get; set; }
}