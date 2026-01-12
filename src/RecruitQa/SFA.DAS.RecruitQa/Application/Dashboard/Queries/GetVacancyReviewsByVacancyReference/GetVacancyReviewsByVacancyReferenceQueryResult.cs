using SFA.DAS.RecruitQa.InnerApi.Responses;

namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsByVacancyReference;

public class GetVacancyReviewsByVacancyReferenceQueryResult
{
    public required List<GetVacancyReviewResponse> VacancyReviews { get; set; }
}
