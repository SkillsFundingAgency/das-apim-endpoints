using System.Collections.Generic;

namespace SFA.DAS.RecruitQa.Api.Models;

public class GetVacancyReviewsApiResponse
{
    public required List<VacancyReviewDto> VacancyReviews { get; set; }
}
