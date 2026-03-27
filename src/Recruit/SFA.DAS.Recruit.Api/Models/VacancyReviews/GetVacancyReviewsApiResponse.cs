using System.Collections.Generic;

namespace SFA.DAS.Recruit.Api.Models.VacancyReviews;

public class GetVacancyReviewsApiResponse
{
    public required List<VacancyReviewDto> VacancyReviews { get; set; }
}