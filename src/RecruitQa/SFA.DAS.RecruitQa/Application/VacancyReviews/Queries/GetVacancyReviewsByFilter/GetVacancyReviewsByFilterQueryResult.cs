using SFA.DAS.RecruitQa.InnerApi.Responses;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByFilter;

public class GetVacancyReviewsByFilterQueryResult
{
    public required List<GetVacancyReviewResponse> VacancyReviews { get; set; }
}
